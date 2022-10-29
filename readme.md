# Docs



### How to use my api

Address: api.che10vek.xyz

Base usage: api/{action}

Authentication: api/Login

| Api             | Type | Response code                           |
|-----------------|------|-----------------------------------------|
| [Login](#login) | Post | Ok(200), Unauthorized(401)              |
| [RunCommand]()  | Post | OK(200)                                 |
___

### Login

#### Request
`{ "login": string, "password": string }`

#### Response
`{ "token": string }`

### RunCommand

#### Request
`{ "text": string, "username": text }`

#### Response
`{ "text": string }`

#### Error
If an internal error has occurred text will be empty or null
