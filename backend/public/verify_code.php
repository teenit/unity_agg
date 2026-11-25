<?php
// Крок 2: фронт надсилає email + код -> якщо код правильний, видаємо токен.
// POST {"email": "...", "code": "123456"}

require __DIR__ . '/../auth.php';

requireMethod('POST');

$data  = readJson();
$email = readEmail($data);
$code  = $data['code'] ?? null;

if ($email === null || !is_string($code) || !preg_match('/^\d{6}$/', $code)) {
    jsonResponse(['success' => false, 'message' => 'Expected JSON: {"email": "...", "code": "6 digits"}'], 400);
}

switch (verifyLoginCode($email, $code)) {
    case 'not_found':
        jsonResponse(['success' => false, 'message' => 'Code not requested or expired'], 401);
    case 'too_many':
        jsonResponse(['success' => false, 'message' => 'Too many attempts, request a new code'], 429);
    case 'wrong':
        jsonResponse(['success' => false, 'message' => 'Wrong code'], 401);
}

$user  = findOrCreateUser($email);
$token = createToken((int) $user['id']);

jsonResponse(['success' => true, 'token' => $token, 'user' => publicUser($user)]);
