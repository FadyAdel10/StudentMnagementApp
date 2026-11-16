# Student & Skill Management API Documentation for Frontend

> **Generated / Updated**: November 2026  
> **Backend Architecture**: ASP.NET Core Web API (Clean Architecture + CQRS)  
> **Server Base URL**:  
> - HTTPS: `https://localhost:7044`  
> - HTTP: `http://localhost:5118`  
> - Swagger UI: `https://localhost:7044/swagger`  

---

## 1. Project Overview & Integration Notes

### 1.1 CORS Status
- **CORS is enabled** on the backend with policy `"FrontendPolicy"` allowing any origin, header, and method.
- Frontend dev servers (Vite `http://localhost:5173`, React `http://localhost:3000`, Angular `http://localhost:4200`) can communicate directly without CORS proxy.

### 1.2 Authentication & Authorization Status
- **Authentication system is fully live** under `/api/auth` (ASP.NET Core Identity + JWT Bearer + Refresh Token rotation).
- **`StudentsController` (`/api/students`) is secured with `[Authorize]`**:
  - Requires `Authorization: Bearer <accessToken>` header on all requests.
  - Returns `401 Unauthorized` if token is missing, expired, or invalid.
- **`SkillsController` (`/api/skills`) is public**:
  - Can be accessed without a token.
- **`AuthController` (`/api/auth`) is public**:
  - Handles user registration, login, token refresh, and logout.

### 1.3 Token Lifetime & Security Details
- **Access Token**: JWT format, validity: **60 minutes**.
- **Refresh Token**: Opaque secure string, validity: **7 days**.
- **Refresh Token Rotation**: Each call to `/api/auth/refresh` revokes the old refresh token and returns a new access token + new refresh token.
- **Reuse Detection / Revocation**: If an already-revoked refresh token is submitted, the server detects potential token compromise and revokes the entire token family.

### 1.4 Recommended Frontend Auth Workflow
```
[Login / Register] ──> Receive { accessToken, refreshToken, userId, email }
                            │
                            ▼
              Store tokens (e.g. Memory / Secure Storage)
                            │
                            ▼
   Attach `Authorization: Bearer <accessToken>` to secured endpoints (/api/students)
                            │
               Is response 401 Unauthorized?
                     ├── Yes ──> Call POST /api/auth/refresh with { refreshToken }
                     │             ├── Success (200) ──> Update tokens, retry original request
                     │             └── Failure (401) ──> Clear storage & redirect to /login
                     └── No  ──> Continue normally
```

### 1.5 Common Data Types & Enums
- **IDs**: UUID / GUID string (e.g., `"3fa85f64-5717-4562-b3fc-2c963f66afa6"`).
- **`ProficiencyLevel` Enum**:
  ```json
  1 = Beginner
  2 = Intermediate
  3 = Advanced
  4 = Expert
  ```

---

## 2. Authentication API (`/api/auth`)
> **Auth Required**: No (Public)

---

### 2.1 Register New User
Registers a new user account with email and password, and automatically issues access and refresh tokens upon success.

- **Method**: `POST`
- **URL**: `/api/auth/register`
- **Headers**: `Content-Type: application/json`
- **Request Body**:
```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```
| Field | Type | Required | Notes |
|---|---|---|---|
| `email` | string | **Yes** | Valid email address |
| `password` | string | **Yes** | Must satisfy ASP.NET Core Identity rules (uppercase, lowercase, digit, non-alphanumeric, min 6 chars) |

- **Response `200 OK` (Registration Succeeded)**:
```json
{
  "userId": "4c944122-bc57-410a-b3aa-ee1129f12345",
  "email": "user@example.com",
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "d8e3b794-01ac-469a-9e12-32b0d0a5127e"
}
```
- **Response `400 Bad Request` (Validation / Password Rule Failure)**:
```json
{
  "errors": [
    "Passwords must have at least one non alphanumeric character.",
    "Passwords must have at least one digit ('0'-'9').",
    "Passwords must have at least one uppercase ('A'-'Z')."
  ]
}
```

---

### 2.2 Login
Authenticates an existing user and returns access and refresh tokens.

- **Method**: `POST`
- **URL**: `/api/auth/login`
- **Headers**: `Content-Type: application/json`
- **Request Body**:
```json
{
  "email": "user@example.com",
  "password": "Password123!"
}
```
| Field | Type | Required | Notes |
|---|---|---|---|
| `email` | string | **Yes** | Registered email |
| `password` | string | **Yes** | Account password |

- **Response `200 OK` (Login Succeeded)**:
```json
{
  "userId": "4c944122-bc57-410a-b3aa-ee1129f12345",
  "email": "user@example.com",
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "d8e3b794-01ac-469a-9e12-32b0d0a5127e"
}
```
- **Response `401 Unauthorized` (Invalid Credentials)**:
```json
{
  "message": "Invalid email or password."
}
```

---

### 2.3 Refresh Access Token
Issues a new access token and rotated refresh token using an active refresh token.

- **Method**: `POST`
- **URL**: `/api/auth/refresh`
- **Headers**: `Content-Type: application/json`
- **Request Body**:
```json
{
  "refreshToken": "d8e3b794-01ac-469a-9e12-32b0d0a5127e"
}
```
| Field | Type | Required | Notes |
|---|---|---|---|
| `refreshToken` | string | **Yes** | Active refresh token obtained from login/register/refresh |

- **Response `200 OK` (Refreshed Successfully)**:
```json
{
  "userId": "4c944122-bc57-410a-b3aa-ee1129f12345",
  "email": "user@example.com",
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "fa91b351-5a33-4f90-87ef-98f55da2841f"
}
```
- **Response `401 Unauthorized` (Expired / Revoked / Invalid Token)**:
```json
{
  "message": "Invalid or expired refresh token."
}
```

---

### 2.4 Logout (Revoke Refresh Token)
Revokes the refresh token family to prevent further token renewal when the user logs out.

- **Method**: `POST`
- **URL**: `/api/auth/logout`
- **Headers**: `Content-Type: application/json`
- **Request Body**:
```json
{
  "refreshToken": "fa91b351-5a33-4f90-87ef-98f55da2841f"
}
```
| Field | Type | Required | Notes |
|---|---|---|---|
| `refreshToken` | string | **Yes** | Refresh token to invalidate |

- **Response `204 No Content`**: Token revoked successfully. Frontend should purge stored credentials and tokens.
- **Response `401 Unauthorized`**: Token invalid or already revoked.
```json
{
  "message": "Error in revoke refresh token"
}
```

---

## 3. Skills API (`/api/skills`)
> **Auth Required**: No (Public)

---

### 3.1 Get All Skills
Returns a list of all skills registered in the catalog.

- **Method**: `GET`
- **URL**: `/api/skills`
- **Headers**: `Accept: application/json`
- **Response `200 OK`**:
```json
[
  {
    "id": "e6f42b32-9cb7-4a0e-9472-88f55da2841f",
    "name": "C# / .NET",
    "description": "Object-oriented programming and web development with .NET Core",
    "isActive": true
  },
  {
    "id": "7b8d1e21-5a33-4f90-87ef-32b0d0a5127e",
    "name": "Angular",
    "description": "Frontend SPA framework",
    "isActive": true
  }
]
```

---

### 3.2 Get Skill By ID
Retrieves details for a single skill.

- **Method**: `GET`
- **URL**: `/api/skills/{id}`
- **URL Parameters**: `id` (GUID)
- **Response `200 OK`**:
```json
{
  "id": "e6f42b32-9cb7-4a0e-9472-88f55da2841f",
  "name": "C# / .NET",
  "description": "Object-oriented programming and web development with .NET Core",
  "isActive": true
}
```
- **Response `404 Not Found`**: Returned if the ID does not exist.

---

### 3.3 Create Skill
Registers a new skill in the catalog.

- **Method**: `POST`
- **URL**: `/api/skills`
- **Headers**: `Content-Type: application/json`
- **Request Body**:
```json
{
  "name": "React",
  "description": "Component-based UI development"
}
```
| Field | Type | Required | Max Length | Notes |
|---|---|---|---|---|
| `name` | string | **Yes** | 100 | Skill name |
| `description` | string | No | 500 | Optional description |

- **Response `201 Created`**:
```json
{
  "id": "3b29c91f-0e12-4217-91a5-e325170dcb01"
}
```
- **Response Header**: `Location: /api/skills/{new-id}`

---

### 3.4 Update Skill
Updates an existing skill's name and description.

- **Method**: `PUT`
- **URL**: `/api/skills/{id}`
- **URL Parameters**: `id` (GUID)
- **Request Body**:
```json
{
  "name": "React.js",
  "description": "Modern frontend development with React & hooks"
}
```
- **Response `204 No Content`**: Update succeeded.
- **Response `404 Not Found`**: Skill not found.

---

### 3.5 Change Skill Status (Activate / Deactivate)
Toggles skill status. Inactive skills cannot be newly assigned to students.

- **Method**: `PATCH`
- **URL**: `/api/skills/{id}/status`
- **URL Parameters**: `id` (GUID)
- **Request Body**:
```json
{
  "isActive": false
}
```
- **Response `204 No Content`**: Status changed successfully.
- **Response `404 Not Found`**: Skill not found.

---

## 4. Students API (`/api/students`)
> **Auth Required**: Yes (`Authorization: Bearer <accessToken>`)

---

### 4.1 Get All Students (With Search & Filter)
Returns students list, each including their assigned skills and proficiency levels.

- **Method**: `GET`
- **URL**: `/api/students`
- **Headers**: `Authorization: Bearer <accessToken>`
- **Query Parameters**:
  - `searchTerm` *(optional, string)*: Filters by First Name, Last Name, or Email.
  - `isActive` *(optional, boolean)*: Filters by active status (`true` or `false`).
- **Example URL**: `/api/students?searchTerm=ahmed&isActive=true`
- **Response `200 OK`**:
```json
[
  {
    "id": "b3e05a8f-22a4-473d-9d11-3e4cb45df749",
    "firstName": "Ahmed",
    "lastName": "Ali",
    "email": "ahmed.ali@example.com",
    "isActive": true,
    "skills": [
      {
        "skillId": "e6f42b32-9cb7-4a0e-9472-88f55da2841f",
        "skillName": "C# / .NET",
        "proficiencyLevel": 3,
        "proficiencyLevelName": "Advanced"
      },
      {
        "skillId": "7b8d1e21-5a33-4f90-87ef-32b0d0a5127e",
        "skillName": "Angular",
        "proficiencyLevel": 2,
        "proficiencyLevelName": "Intermediate"
      }
    ]
  }
]
```
- **Response `401 Unauthorized`**: Bearer token missing or invalid.

---

### 4.2 Get Student By ID
Returns detailed profile for a specific student, including all their assigned skills.

- **Method**: `GET`
- **URL**: `/api/students/{id}`
- **Headers**: `Authorization: Bearer <accessToken>`
- **URL Parameters**: `id` (GUID)
- **Response `200 OK`**:
```json
{
  "id": "b3e05a8f-22a4-473d-9d11-3e4cb45df749",
  "firstName": "Ahmed",
  "lastName": "Ali",
  "email": "ahmed.ali@example.com",
  "isActive": true,
  "skills": [
    {
      "skillId": "e6f42b32-9cb7-4a0e-9472-88f55da2841f",
      "skillName": "C# / .NET",
      "proficiencyLevel": 3,
      "proficiencyLevelName": "Advanced"
    }
  ]
}
```
- **Response `401 Unauthorized`**: Bearer token missing or invalid.
- **Response `404 Not Found`**: Student does not exist.

---

### 4.3 Create Student
Enrolls a new student. Initial status defaults to `true` (Active).

- **Method**: `POST`
- **URL**: `/api/students`
- **Headers**: 
  - `Content-Type: application/json`
  - `Authorization: Bearer <accessToken>`
- **Request Body**:
```json
{
  "firstName": "Sara",
  "lastName": "Hassan",
  "email": "sara.hassan@example.com"
}
```
| Field | Type | Required | Max Length | Notes |
|---|---|---|---|---|
| `firstName` | string | **Yes** | 100 | Cannot be empty/whitespace |
| `lastName` | string | **Yes** | 100 | Cannot be empty/whitespace |
| `email` | string | **Yes** | 255 | Must be valid email address format |

- **Response `201 Created`**:
```json
{
  "id": "6d1234ef-5678-90ab-cdef-1234567890ab"
}
```
- **Response Header**: `Location: /api/students/{new-id}`

---

### 4.4 Update Student Profile
Updates personal information for an active student.

- **Method**: `PUT`
- **URL**: `/api/students/{id}`
- **Headers**: 
  - `Content-Type: application/json`
  - `Authorization: Bearer <accessToken>`
- **URL Parameters**: `id` (GUID)
- **Request Body**:
```json
{
  "firstName": "Sara",
  "lastName": "Mohamed",
  "email": "sara.mohamed@example.com"
}
```
- **Response `204 No Content`**: Profile updated successfully.
- **Response `400 Bad Request`**: Validation failed or student is Inactive (inactive students cannot update profiles until reactivated).
- **Response `401 Unauthorized`**: Bearer token missing or invalid.
- **Response `404 Not Found`**: Student not found.

---

### 4.5 Change Student Status (Activate / Deactivate)
Toggles a student between active and inactive.

- **Method**: `PATCH`
- **URL**: `/api/students/{id}/status`
- **Headers**: 
  - `Content-Type: application/json`
  - `Authorization: Bearer <accessToken>`
- **URL Parameters**: `id` (GUID)
- **Request Body**:
```json
{
  "isActive": false
}
```
- **Response `204 No Content`**: Status changed successfully.
- **Response `401 Unauthorized`**: Bearer token missing or invalid.
- **Response `404 Not Found`**: Student not found.

---

## 5. Student Skills Sub-Resource API (`/api/students/{studentId}/skills`)
> **Auth Required**: Yes (`Authorization: Bearer <accessToken>`)

---

### 5.1 Get All Skills for a Student
- **Method**: `GET`
- **URL**: `/api/students/{studentId}/skills`
- **Headers**: `Authorization: Bearer <accessToken>`
- **URL Parameters**: `studentId` (GUID)
- **Response `200 OK`**:
```json
[
  {
    "skillId": "e6f42b32-9cb7-4a0e-9472-88f55da2841f",
    "skillName": "C# / .NET",
    "proficiencyLevel": 3,
    "proficiencyLevelName": "Advanced"
  }
]
```

---

### 5.2 Assign Skill to Student
Assigns a new skill from the catalog to the student with a chosen proficiency level.

- **Method**: `POST`
- **URL**: `/api/students/{studentId}/skills`
- **Headers**: 
  - `Content-Type: application/json`
  - `Authorization: Bearer <accessToken>`
- **URL Parameters**: `studentId` (GUID)
- **Request Body**:
```json
{
  "skillId": "7b8d1e21-5a33-4f90-87ef-32b0d0a5127e",
  "proficiency": 2
}
```
| Field | Type | Required | Values | Notes |
|---|---|---|---|---|
| `skillId` | string (GUID) | **Yes** | Existing active skill ID | Cannot be empty |
| `proficiency` | integer | **Yes** | `1`, `2`, `3`, `4` | `1`=Beginner, `2`=Intermediate, `3`=Advanced, `4`=Expert |

- **Response `204 No Content`**: Assigned successfully.
- **Response `400 Bad Request`**: Duplicate assignment or invalid proficiency value.
- **Response `401 Unauthorized`**: Bearer token missing or invalid.
- **Response `404 Not Found`**: Student or Skill not found.

---

### 5.3 Update Student Skill Proficiency Level
Updates the proficiency level of an already assigned skill.

- **Method**: `PUT`
- **URL**: `/api/students/{studentId}/skills/{skillId}`
- **Headers**: 
  - `Content-Type: application/json`
  - `Authorization: Bearer <accessToken>`
- **URL Parameters**:
  - `studentId` (GUID)
  - `skillId` (GUID)
- **Request Body**:
```json
{
  "proficiency": 3
}
```
- **Response `204 No Content`**: Updated successfully.
- **Response `400 Bad Request`**: Invalid proficiency value.
- **Response `401 Unauthorized`**: Bearer token missing or invalid.
- **Response `404 Not Found`**: Student or Skill assignment not found.

---

### 5.4 Remove Skill from Student
Removes an assigned skill from the student's profile.

- **Method**: `DELETE`
- **URL**: `/api/students/{studentId}/skills/{skillId}`
- **Headers**: `Authorization: Bearer <accessToken>`
- **URL Parameters**:
  - `studentId` (GUID)
  - `skillId` (GUID)
- **Response `204 No Content`**: Removed successfully.
- **Response `401 Unauthorized`**: Bearer token missing or invalid.
- **Response `404 Not Found`**: Assignment not found.

---

## 6. Summary Endpoint Cheat Sheet for Frontend

| Method | Endpoint | Auth | Purpose |
|---|---|---|---|
| **`POST`** | `/api/auth/register` | ❌ No | Register new user + get tokens |
| **`POST`** | `/api/auth/login` | ❌ No | Login + get access & refresh tokens |
| **`POST`** | `/api/auth/refresh` | ❌ No | Refresh access token via refresh token |
| **`POST`** | `/api/auth/logout` | ❌ No | Revoke refresh token |
| **`GET`** | `/api/skills` | ❌ No | List all skills |
| **`POST`** | `/api/skills` | ❌ No | Create a new skill |
| **`GET`** | `/api/skills/{id}` | ❌ No | Get single skill details |
| **`PUT`** | `/api/skills/{id}` | ❌ No | Update skill |
| **`PATCH`** | `/api/skills/{id}/status` | ❌ No | Toggle active/inactive skill |
| **`GET`** | `/api/students?searchTerm=&isActive=` | 🔒 Bearer | List students + search & filter |
| **`POST`** | `/api/students` | 🔒 Bearer | Create new student |
| **`GET`** | `/api/students/{id}` | 🔒 Bearer | Get student details + their skills |
| **`PUT`** | `/api/students/{id}` | 🔒 Bearer | Update student profile |
| **`PATCH`** | `/api/students/{id}/status` | 🔒 Bearer | Toggle active/inactive student |
| **`GET`** | `/api/students/{studentId}/skills` | 🔒 Bearer | List skills assigned to a student |
| **`POST`** | `/api/students/{studentId}/skills` | 🔒 Bearer | Assign skill with proficiency (1–4) |
| **`PUT`** | `/api/students/{studentId}/skills/{skillId}`| 🔒 Bearer | Update skill proficiency (1–4) |
| **`DELETE`** | `/api/students/{studentId}/skills/{skillId}`| 🔒 Bearer | Remove skill from student |
