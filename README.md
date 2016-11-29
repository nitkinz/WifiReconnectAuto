# WifiReconnectAuto

В классе Program в nameWifi указываем имя wifi.

В поле key задается пароль.

Сейчас программа настроена на сеть WPA2-Personal. Если у вас другая, необходимо передать свой профиль. https://msdn.microsoft.com/ru-RU/library/windows/desktop/aa369853(v=vs.85).aspx

УСТАНОВКА:
1) Сбилдить и зайти в планировщик windows().

2) Create Task

3) Вкладка general. Указываем имя. снизу ставим чекбокс в Run whether user is logged on or not и Configure for: Windows 10

4) На вкладке Triggers создаем новый и в Begin the task указываем At startup. Repeat task every 15 minutes for a duration of Indefinitely.

5) В Action создаем новый и указываем в Action: Start a program и в Program/script:  передаем путь к exe программы.

6) Нажимаем ок. В логах можно посмотреть запускается ли программа через заданный интервал.
