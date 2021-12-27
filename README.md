# ASP.NET Core 6

## Empty Project

create new ASP.NET Core 6 MVC Controller project

## Windows Authentication Added

create new ASP.NET Core 6 MVC Controller project, selecting windows authentication

## Authorization Added

Play around with several scenario's in _custom middleware_ and _controller's attribute_:

- add role 'dummy_2', configure controller Authorize attribute using 'dummy_2': access
- add role 'dummy_2', configure controller Authorize attribute using 'dummy_3': no access
- add role 'dummy_2', configure controller Authorize attribute using 'dummy_2' OR 'dummy_3': access
- add role 'dummy_2' or 'dummy_3', configure controller Authorize attribute using 'dummy_2' AND 'dummy_3': no access
- add role 'dummy_2' and 'dummy_3', configure controller Authorize attribute using 'dummy_2' AND 'dummy_3': access

## Status

- [x] works with Kestrel (debug, no client cert)
- [x] works with IIS Express (debug, no client cert)
- [x] works (theoretically - cert is beeing requested) with Kestrel (debug, client cert)      - todo: how to create a client certificate?
- [ ] works with IIS Express (debug, client cert)  - todo: how can IIS Express request Client certificate? how to create a client certificate?
- [ ] does not work with local IIS (windows auth asks for user/password, but works sometimes with IE11!?)

## Information
