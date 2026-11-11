# Student & Skill Management API Documentation for Frontend

> **Generated**: November 2026  
> **Backend Architecture**: ASP.NET Core Web API (Clean Architecture + CQRS)  
> **Server Base URL**:  
> - HTTPS: `https://localhost:7044`  
> - HTTP: `http://localhost:5118`  
> - Swagger UI: `https://localhost:7044/swagger`  

---

## 1. Current Project Situation & Integration Notes

### 1.1 CORS Status
- **CORS is enabled** on the backend with policy `"FrontendPolicy"` allowing any origin, header, and method.
- Frontend development servers (Vite `http://localhost:5173`, React `http://localhost:3000`, Angular `http://localhost:4200`) can communicate directly without CORS proxy.

### 1.2 Authentication & Authorization Status
- **`StudentsController` is secured with `[Authorize]`**:
  - All endpoints under `/api/students` currently require an `Authorization: Bearer <token>` header.
- **`SkillsController` is public**:
  - `/api/skills` can be accessed without a token.
- **`AuthenticationController` is currently in progress**:
  - The JWT infrastructure is ready in backend `Program.cs`, but `Login` and `Register` endpoints are being wired up in `AuthenticationController`.
  - *Tip for Frontend Dev*: When testing `/api/students`, ensure either a valid JWT token is attached in the header or the `[Authorize]` attribute is temporarily commented out for local development.

### 1.3 Common Data Types & Enums
- **IDs**: UUID / GUID string (e.g., `"3fa85f64-5717-4562-b3fc-2c963f66afa6"`).
- **`ProficiencyLevel` Enum**:
  ```json
  1 = Beginner
  2 = Intermediate
  3 = Advanced
  4 = Expert
  ```

---

## 2. Skills API (`/api/skills`)
> **Auth Required**: No (Public)

---

### 2.1 Get All Skills
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

### 2.2 Get Skill By ID
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

### 2.3 Create Skill
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

### 2.4 Update Skill
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

### 2.5 Change Skill Status (Activate / Deactivate)
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

## 3. Students API (`/api/students`)
> **Auth Required**: Yes (`Bearer <token>`)

---

### 3.1 Get All Students (With Search & Filter)
Returns students list, each including their assigned skills and proficiency levels.

- **Method**: `GET`
- **URL**: `/api/students`
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

---

### 3.2 Get Student By ID
Returns detailed profile for a specific student, including all their skills.

- **Method**: `GET`
- **URL**: `/api/students/{id}`
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
- **Response `404 Not Found`**: Student does not exist.

---

### 3.3 Create Student
Enrolls a new student. Initial status defaults to `true` (Active).

- **Method**: `POST`
- **URL**: `/api/students`
- **Headers**: `Content-Type: application/json`
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

### 3.4 Update Student Profile
Updates personal information for an active student.

- **Method**: `PUT`
- **URL**: `/api/students/{id}`
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
- **Response `404 Not Found`**: Student not found.

---

### 3.5 Change Student Status (Activate / Deactivate)
Toggles a student between active and inactive.

- **Method**: `PATCH`
- **URL**: `/api/students/{id}/status`
- **URL Parameters**: `id` (GUID)
- **Request Body**:
```json
{
  "isActive": false
}
```
- **Response `204 No Content`**: Status changed successfully.
- **Response `404 Not Found`**: Student not found.

---

## 4. Student Skills Sub-Resource API (`/api/students/{studentId}/skills`)
> **Auth Required**: Yes (`Bearer <token>`)

---

### 4.1 Get All Skills for a Student
- **Method**: `GET`
- **URL**: `/api/students/{studentId}/skills`
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

### 4.2 Assign Skill to Student
Assigns a new skill from catalog to the student with a chosen proficiency level.

- **Method**: `POST`
- **URL**: `/api/students/{studentId}/skills`
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
- **Response `404 Not Found`**: Student or Skill not found.

---

### 4.3 Update Student Skill Proficiency Level
Updates the proficiency level of an already assigned skill.

- **Method**: `PUT`
- **URL**: `/api/students/{studentId}/skills/{skillId}`
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
- **Response `404 Not Found`**: Student or Skill assignment not found.

---

### 4.4 Remove Skill from Student
Removes an assigned skill from the student's profile.

- **Method**: `DELETE`
- **URL**: `/api/students/{studentId}/skills/{skillId}`
- **URL Parameters**:
  - `studentId` (GUID)
  - `skillId` (GUID)
- **Response `204 No Content`**: Removed successfully.
- **Response `404 Not Found`**: Assignment not found.

---

## 5. Summary Endpoint Cheat Sheet for Frontend

| Method | Endpoint | Auth | Purpose |
|---|---|---|---|
| `GET` | `/api/skills` | ❌ No | List all skills |
| `POST` | `/api/skills` | ❌ No | Create a new skill |
| `GET` | `/api/skills/{id}` | ❌ No | Get single skill |
| `PUT` | `/api/skills/{id}` | ❌ No | Update skill |
| `PATCH` | `/api/skills/{id}/status` | ❌ No | Toggle active/inactive skill |
| `GET` | `/api/students?searchTerm=&isActive=` | 🔒 Yes | List students + search & filter |
| `POST` | `/api/students` | 🔒 Yes | Create new student |
| `GET` | `/api/students/{id}` | 🔒 Yes | Get student details + their skills |
| `PUT` | `/api/students/{id}` | 🔒 Yes | Update student profile |
| `PATCH` | `/api/students/{id}/status` | 🔒 Yes | Toggle active/inactive student |
| `GET` | `/api/students/{studentId}/skills` | 🔒 Yes | List skills of a student |
| `POST` | `/api/students/{studentId}/skills` | 🔒 Yes | Assign skill with proficiency |
| `PUT` | `/api/students/{studentId}/skills/{skillId}`| 🔒 Yes | Update proficiency (1–4) |
| `DELETE` | `/api/students/{studentId}/skills/{skillId}`| 🔒 Yes | Remove skill from student |
