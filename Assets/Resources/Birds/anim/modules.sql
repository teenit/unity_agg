-- phpMyAdmin SQL Dump
-- version 5.2.3
-- https://www.phpmyadmin.net/
--
-- Хост: teen.mysql.ukraine.com.ua:3306
-- Время создания: Май 14 2026 г., 01:36
-- Версия сервера: 5.7.44-54-log
-- Версия PHP: 7.4.33

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `teen_admincrm`
--

--
-- Дамп данных таблицы `modules`
--

INSERT INTO `modules` (`id`, `key`, `name`, `description`, `is_active`, `created_at`, `updated_at`) VALUES
(1, 'tasks', 'Таск-менеджер', '', 1, '2026-05-13 02:39:55', '2026-05-13 02:39:55'),
(2, 'transactions', 'Транзакції', '', 1, '2026-05-13 02:40:19', '2026-05-13 02:40:19'),
(3, 'resources', 'Ресурси', '', 1, '2026-05-13 02:41:38', '2026-05-13 02:41:38'),
(4, 'groups', 'Групи', '', 1, '2026-05-13 02:42:14', '2026-05-13 02:42:14'),
(5, 'ancets', 'Анкети', '', 1, '2026-05-13 02:42:36', '2026-05-13 02:42:36'),
(6, 'statistic', 'Статистика', '', 1, '2026-05-13 02:42:53', '2026-05-13 02:42:53'),
(7, 'projects', 'Проєкти', '', 1, '2026-05-13 02:43:18', '2026-05-13 02:43:18'),
(8, 'inventory', 'Інвентаризація', '', 1, '2026-05-13 02:43:40', '2026-05-13 02:43:40'),
(9, 'cases', 'Кейси', '', 1, '2026-05-13 02:44:09', '2026-05-13 02:44:09'),
(10, 'events', 'Івенти', '', 1, '2026-05-13 02:46:40', '2026-05-13 02:46:40'),
(11, 'contacts', 'Телефонна книга', '', 1, '2026-05-13 02:47:03', '2026-05-13 02:47:03'),
(12, 'imports', 'Імпорт даних', '', 1, '2026-05-13 02:48:33', '2026-05-13 02:48:33');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
