<?php
// Тестова авторизація: логін/пароль з .env, без БД і без шифрування.

function login(string $login, string $password): bool
{
    $envLogin    = getenv('AUTH_LOGIN');
    $envPassword = getenv('AUTH_PASSWORD');
    $isLogin = false;

    if ($login === $envLogin && $password === $envPassword) {
        $isLogin = true;
    } else {
        $isLogin = false;
    }

    return $isLogin;
}
