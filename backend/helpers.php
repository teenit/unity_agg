<?php
// Спільні хелпери: час, JSON-запити та відповіді.

// Увесь час у проєкті рахуємо в PHP, а не в MySQL
date_default_timezone_set(getenv('APP_TIMEZONE') ?: 'Europe/Kyiv');

// Поточний (або зсунутий) час у форматі DATETIME для MySQL.
// Приклад: dbTime('+10 minutes')
function dbTime(string $modify = 'now'): string
{
    return (new DateTimeImmutable($modify))->format('Y-m-d H:i:s');
}

// Надіслати JSON-відповідь і завершити скрипт
function jsonResponse(array $data, int $status = 200): never
{
    http_response_code($status);
    header('Content-Type: application/json');
    echo json_encode($data);
    exit;
}

// Дозволити тільки вказаний HTTP-метод
function requireMethod(string $method): void
{
    if ($_SERVER['REQUEST_METHOD'] !== $method) {
        jsonResponse(['success' => false, 'message' => 'Method not allowed'], 405);
    }
}

// Прочитати JSON з тіла запиту (або null, якщо там не JSON-об'єкт)
function readJson(): ?array
{
    $data = json_decode(file_get_contents('php://input'), true);
    return is_array($data) ? $data : null;
}

// Дістати й нормалізувати email з даних запиту (або null, якщо він некоректний)
function readEmail(?array $data): ?string
{
    if (!isset($data['email']) || !is_string($data['email'])) {
        return null;
    }
    $email = strtolower(trim($data['email']));
    return filter_var($email, FILTER_VALIDATE_EMAIL) ? $email : null;
}
