Asset Management
=

Yet another experiment in creating a CQRS-like application.

Данный проект это попытка смоделировать приложение для учета ИТ-активов (программных и аппаратных) и отслеживания их изменений с течением времени.
Предполагается что сама процедура обнаружения и идентификации активов выполняется сторонним приложением.

# Disclaimer

The infrastructure this project is build upon is not intended for production use. It's sole purpose is to demonstrate and play with message flow, handlers and long running processes (aka sagas).

# Definitions

Актив - единица учета, программная или аппаратная.

Хост - физическое или логическое объединение активов по каким то идентификационным признакам.

