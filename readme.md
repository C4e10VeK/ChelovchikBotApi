# Docs

<!-- TOC -->
* [Docs](#docs)
    * [How to use my api](#how-to-use-my-api)
    * [Tip/GetTip](#tipgettip)
      * [Request](#request)
      * [Response](#response)
    * [Feed/GetTop](#feedgettop)
      * [Request](#request)
      * [Response](#response)
    * [Feed/Feed (Put)](#feedfeedput)
      * [Request](#request)
      * [Response](#response)
    * [Feed/Feed (Get)](#feedfeedget)
      * [Request](#request)
      * [Response](#response)
    * [Feed/GetFeedStatus](#feedgetfeedstatus)
      * [Request](#request)
      * [Response](#response)
      * [Error](#error)
    * [Feed/Add](#feedadd)
      * [Request](#request)
      * [Response](#response)
<!-- TOC -->

### How to use my api

Ip: 185.14.45.134

Base usage: api/{controller}/{action}

Authentication: api/Login

| Api                                      | Type | Respnse code                            |
|------------------------------------------|------|-----------------------------------------|
| [Login](#login)                          | Post | Ok(200), Unauthorized(401)              |
| [Tip/GetTip](#tipgettip)                 | Post | OK(200)                                 |
| [Feed/GetTop](#feedgettop)               | Get  | OK(200)                                 |
| [Feed/Feed](#feedfeedput)                | Put  | OK(200), NotFound(404)                  |
| [Feed/Feed](#feedfeedget)                | Get  | Ok(200)                                 |
| [Feed/GetFeedStatus](#feedgetfeedstatus) | Post | OK(200), NotFound(404), BadRequest(400) |
| [Feed/Add](#feedadd)                     | Post | OK(200), NotFound(404), BadRequest(400) |
___

### Login

#### Request
`{ "login": string, "password": string }`

#### Response
`{ "token": string }`

### Tip/GetTip

#### Request
`{ "qyery": string }`

#### Response
`{ "qyery": string, "text": string }`

### Feed/GetTop

#### Request
None

#### Response
`{ "data": array of SmileTop }`

SmileTop:
`{ "index": int, "index_str": string, "smile": string, "size": double }`

### Feed/Feed(Put)

#### Request
`{ "username": string, "smile_name": string }`

#### Response
`{ "text": string }`

### Feed/Feed(Get)

#### Request
None

#### Response
`{ "text": string }`

### Feed/GetFeedStatus

#### Request
`{ "username": string }`

#### Response
`{ "username": string, "smiles": array of strings, smile name }`

#### Error
`{ "text": string }`

### Feed/Add
#### Request
`{ "username": string, "smile_name": string }`

#### Response
`{ "text": string }`
