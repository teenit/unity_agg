<?php
require __DIR__ . '/../auth.php';

header('Content-Type: application/json');

if ($_SERVER['REQUEST_METHOD'] !== 'POST') {
    http_response_code(405);
    echo json_encode(['success' => false, 'message' => 'Method not allowed']);
    exit;
}

$data = json_decode(file_get_contents('php://input'), true);

if (!is_array($data) || !isset($data['login'], $data['password'])
    || !is_string($data['login']) || !is_string($data['password'])) {
    http_response_code(400);
    echo json_encode(['success' => false, 'message' => 'Expected JSON: {"login": "...", "password": "..."}']);
    exit;
}

if (login($data['login'], $data['password'])) {
    echo json_encode(['success' => true]);
} else {
    http_response_code(401);
    echo json_encode(['success' => false, 'message' => 'Invalid login or password']);
}
