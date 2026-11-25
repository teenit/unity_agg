<?php
require __DIR__ . '/../config/database.php';

header('Content-Type: application/json');

try {
    $version = db()->query('SELECT VERSION() AS v')->fetch()['v'];
    echo json_encode(['status' => 'ok', 'php' => PHP_VERSION, 'mysql' => $version]);
} catch (Throwable $e) {
    http_response_code(500);
    echo json_encode(['status' => 'error', 'message' => $e->getMessage()]);
}

