<?php
// Вхід за токеном: фронт надсилає заголовок "Authorization: Bearer <токен>".
// GET -> дані поточного гравця або 401

require __DIR__ . '/../auth.php';

requireMethod('GET');

$user = requireUser();

jsonResponse(['success' => true, 'user' => publicUser($user)]);
