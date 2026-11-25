<?php
// Крок 1: фронт надсилає email -> створюємо (або знаходимо) гравця і код входу.
// POST {"email": "..."}

require __DIR__ . '/../auth.php';

requireMethod('POST');

$email = readEmail(readJson());
if ($email === null) {
    jsonResponse(['success' => false, 'message' => 'Expected JSON: {"email": "..."} with a valid email'], 400);
}

findOrCreateUser($email);
$code = createLoginCode($email);

// TODO: надіслати $code на $email

jsonResponse(['success' => true]);
