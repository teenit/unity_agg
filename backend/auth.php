<?php
// Авторизація без пароля: код з пошти -> токен.

require_once __DIR__ . '/config/database.php';
require_once __DIR__ . '/helpers.php';

const LOGIN_CODE_TTL     = '+10 minutes'; // скільки живе код з пошти
const LOGIN_CODE_MAX_TRY = 5;             // скільки разів можна помилитись з кодом
const TOKEN_TTL          = '+30 days';    // скільки живе токен
const TEST_LOGIN_CODE    = '254254';      // TODO: прибрати, коли почнемо слати справжні коди

// Публічні дані гравця, які віддаємо на фронт
function publicUser(array $user): array
{
    return [
        'id'       => (int) $user['id'],
        'username' => $user['username'],
        'email'    => $user['email'],
        'coins'    => (int) $user['coins'],
    ];
}

// Знайти гравця за email або створити нового
function findOrCreateUser(string $email): array
{
    $stmt = db()->prepare('SELECT * FROM users WHERE email = ?');
    $stmt->execute([$email]);
    $user = $stmt->fetch();

    if ($user) {
        return $user;
    }

    $now = dbTime();
    db()->prepare('INSERT INTO users (email, created_at, updated_at) VALUES (?, ?, ?)')
        ->execute([$email, $now, $now]);

    $stmt = db()->prepare('SELECT * FROM users WHERE id = ?');
    $stmt->execute([db()->lastInsertId()]);
    return $stmt->fetch();
}

// Створити новий код входу для email (старі коди видаляються)
function createLoginCode(string $email): string
{
    // TODO: замінити на str_pad((string) random_int(0, 999999), 6, '0', STR_PAD_LEFT)
    $code = TEST_LOGIN_CODE;

    db()->prepare('DELETE FROM login_codes WHERE email = ?')->execute([$email]);
    db()->prepare('INSERT INTO login_codes (email, code_hash, expires_at, attempts, created_at) VALUES (?, ?, ?, 0, ?)')
        ->execute([$email, hash('sha256', $code), dbTime(LOGIN_CODE_TTL), dbTime()]);

    return $code;
}

// Перевірити код. Повертає 'ok', 'not_found', 'too_many' або 'wrong'
function verifyLoginCode(string $email, string $code): string
{
    $stmt = db()->prepare('SELECT * FROM login_codes WHERE email = ? AND expires_at > ? ORDER BY id DESC LIMIT 1');
    $stmt->execute([$email, dbTime()]);
    $row = $stmt->fetch();

    if (!$row) {
        return 'not_found';
    }

    if ((int) $row['attempts'] >= LOGIN_CODE_MAX_TRY) {
        db()->prepare('DELETE FROM login_codes WHERE id = ?')->execute([$row['id']]);
        return 'too_many';
    }

    if (!hash_equals($row['code_hash'], hash('sha256', $code))) {
        db()->prepare('UPDATE login_codes SET attempts = attempts + 1 WHERE id = ?')->execute([$row['id']]);
        return 'wrong';
    }

    // Код одноразовий
    db()->prepare('DELETE FROM login_codes WHERE id = ?')->execute([$row['id']]);
    return 'ok';
}

// Видати гравцю новий токен (у базі зберігається тільки його хеш)
function createToken(int $userId): string
{
    $token = bin2hex(random_bytes(32));
    $now   = dbTime();

    db()->prepare('INSERT INTO user_tokens (user_id, token_hash, expires_at, created_at) VALUES (?, ?, ?, ?)')
        ->execute([$userId, hash('sha256', $token), dbTime(TOKEN_TTL), $now]);

    db()->prepare('UPDATE users SET last_login_at = ?, updated_at = ? WHERE id = ?')
        ->execute([$now, $now, $userId]);

    return $token;
}

// Дістати токен із заголовка "Authorization: Bearer <токен>"
function bearerToken(): ?string
{
    $header = $_SERVER['HTTP_AUTHORIZATION'] ?? '';

    if ($header === '' && function_exists('getallheaders')) {
        foreach (getallheaders() as $name => $value) {
            if (strcasecmp($name, 'Authorization') === 0) {
                $header = $value;
            }
        }
    }

    return preg_match('/^Bearer\s+([a-f0-9]{64})$/i', trim($header), $m) ? strtolower($m[1]) : null;
}

// Поточний гравець за токеном (або null, якщо токена немає / він недійсний)
function currentUser(): ?array
{
    $token = bearerToken();
    if ($token === null) {
        return null;
    }

    $stmt = db()->prepare(
        'SELECT u.*, t.id AS token_id
         FROM user_tokens t
         JOIN users u ON u.id = t.user_id
         WHERE t.token_hash = ? AND t.expires_at > ?'
    );
    $stmt->execute([hash('sha256', $token), dbTime()]);
    $user = $stmt->fetch();

    if (!$user) {
        return null;
    }

    db()->prepare('UPDATE user_tokens SET last_used_at = ? WHERE id = ?')
        ->execute([dbTime(), $user['token_id']]);

    return $user;
}

// Те саме, що currentUser(), але одразу відповідає 401, якщо гравець не залогінений
function requireUser(): array
{
    $user = currentUser();
    if ($user === null) {
        jsonResponse(['success' => false, 'message' => 'Unauthorized'], 401);
    }
    return $user;
}
