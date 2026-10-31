# Security policy

Report vulnerabilities privately through GitHub's **Report a vulnerability** action in this repository's
Security tab, if private reporting has been enabled. Otherwise contact the maintainer through the
contact details on the jrfrigat GitHub profile to arrange a private channel. Do not post credentials
or exploit details in public issues.

Include the package/runtime version, impact, and a sanitized reproduction. Only the latest release is
maintained; no response-time guarantee is offered.

Tokens belong in environment variables or a secret store. The SDK sends them to BaseAddress;
use the default HTTPS Yandex API address unless deliberately testing with a trusted handler.
Exception.RawResponse can contain private issue data: redact it before logging or sharing.

## Сообщение об уязвимости

Используйте приватный Report a vulnerability на вкладке Security, если он включён. Иначе свяжитесь с
сопровождающим по контактам профиля jrfrigat, чтобы выбрать приватный канал. Не публикуйте токены и
детали эксплуатации в открытых issue. Укажите версию, влияние и обезличенный пример.
Поддерживается последний релиз; гарантированного срока ответа нет.
