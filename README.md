# 🎓 School Management System API

A comprehensive school management system built with .NET 8 Web API, featuring role-based authentication, academic management, and online assessment capabilities.

## 📋 Table of Contents

- [Features](#-features)
- [Architecture](#-architecture)
- [Technologies Used](#-technologies-used)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [API Documentation](#-api-documentation)
- [Authentication](#-authentication)
- [Database Schema](#-database-schema)
- [Contributing](#-contributing)
- [License](#-license)

## ✨ Features

### 🔐 Authentication & Authorization
- JWT-based authentication with refresh tokens
- Role-based access control (Admin, Teacher, Student)
- Secure password policies and user management
- Email-based password reset functionality

### 👥 User Management
- **Admin Panel**: Complete system administration with dashboard analytics
- **Teacher Management**: Teacher profiles, class assignments, and subject management
- **Student Management**: Student enrollment, class assignments, and academic tracking

### 📚 Academic Management
- **Subject Management**: Create and manage academic subjects by grade
- **Lesson Planning**: Organize lessons with materials and content management
- **Class Management**: Dynamic class creation and student-teacher assignments
- **Grade Management**: Multi-grade support with flexible class structures

### 📝 Assessment System
- **Online Quizzes**: Create and manage quizzes with multiple question types
- **Question Bank**: Comprehensive question management system
- **Automated Grading**: Instant quiz scoring and performance tracking
- **Student Analytics**: Detailed performance reports and statistics

### 📊 Additional Features
- **Dashboard Analytics**: Real-time system statistics and user insights
- **File Management**: Upload and manage educational materials
- **Caching**: Redis integration for improved performance
- **Error Handling**: Comprehensive global error handling middleware

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
School Management System/
├── 📁 Domain/                 # Core business logic and entities
├── 📁 AbstractionServices/    # Service interfaces and contracts
├── 📁 Services/               # Business logic implementation
├── 📁 Persistance/            # Data access layer (EF Core, Repositories)
├── 📁 Presentation/           # API Controllers and endpoints
├── 📁 Shared/                 # DTOs, common utilities, and configurations
└── 📁 School_Api/            # Main API project and startup configuration
```

### Design Patterns Used
- **Repository Pattern**: Abstracted data access layer
- **Unit of Work Pattern**: Transaction management and data consistency
- **Dependency Injection**: Loose coupling and testability
- **DTO Pattern**: Data transfer objects for API communication
- **Specification Pattern**: Dynamic query building

## 🛠️ Technologies Used

### Backend
- **.NET 8** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API development
- **Entity Framework Core 8** - ORM for database operations
- **SQL Server** - Primary database
- **Redis** - Caching and session management

### Authentication & Security
- **ASP.NET Core Identity** - User management and authentication
- **JWT (JSON Web Tokens)** - Stateless authentication
- **BCrypt** - Password hashing and security

### Additional Libraries
- **AutoMapper** - Object-to-object mapping
- **Swagger/OpenAPI** - API documentation
- **StackExchange.Redis** - Redis client
- **Microsoft.EntityFrameworkCore.Design** - Database migrations

## 📁 Project Structure

```
School Management System/
├── 📁 Domain/
│   ├── 📁 Contract/           # Repository interfaces and specifications
│   ├── 📁 Exceptions/         # Custom exception classes
│   └── 📁 Models/             # Domain entities and business models
│       ├── 📁 User/           # User-related entities (Student, Teacher, Admin)
│       └── 📁 subject&Lesson/ # Academic content entities
├── 📁 AbstractionServices/    # Service layer interfaces
├── 📁 Services/               # Business logic implementation
│   ├── 📁 Profiles/           # AutoMapper profiles
│   └── 📁 SpecificationsFile/ # Query specifications
├── 📁 Persistance/           # Data access layer
│   ├── 📁 Contexts/          # DbContext and database configuration
│   ├── 📁 Data/              # Entity configurations
│   ├── 📁 Repository/        # Repository implementations
│   └── 📁 SeedingData/       # Initial data seeding
├── 📁 Presentation/          # API Controllers
├── 📁 Shared/                # Shared DTOs and utilities
│   ├── 📁 IdentityDtos/      # User-related DTOs
│   ├── 📁 QuizDto/           # Quiz and assessment DTOs
│   └── 📁 SubjectDtos/       # Academic content DTOs
└── 📁 School_Api/            # Main API project
    ├── 📁 ErrorHnadlingMidlleware/ # Global error handling
    └── 📁 wwwroot/           # Static files and uploads
```

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or Full SQL Server)
- Redis Server
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/school-management-system.git
   cd school-management-system
   ```

2. **Configure the database**
   - Update the connection string in `School_Api/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "SchoolDbConnection": "Your_SQL_Server_Connection_String",
       "Redis": "localhost:6379"
     }
   }
   ```

3. **Run database migrations**
   ```bash
   cd School_Api
   dotnet ef database update
   ```

4. **Configure email settings** (Optional)
   - Update email configuration in `appsettings.json` for password reset functionality

5. **Run the application**
   ```bash
   dotnet run --project School_Api
   ```

6. **Access the API**
   - API: `https://localhost:7134`
   - Swagger UI: `https://localhost:7134/swagger`

## 📚 API Documentation

### Authentication Endpoints
- `POST /api/Auth/Login` - User login
- `POST /api/Auth/RefreshToken` - Refresh JWT token
- `POST /api/Auth/LogOut` - User logout
- `POST /api/Auth/ChangePassword` - Change user password

### Admin Endpoints
- `GET /api/Admin/Dashboard` - Admin dashboard statistics
- `GET /api/Admin/Profile` - Get admin profile
- `PUT /api/Admin/Profile` - Update admin profile
- `GET /api/Admin/Users` - Get all users (paginated)
- `POST /api/Admin/AddStudent` - Add new student
- `POST /api/Admin/AddTeacher` - Add new teacher
- `PUT /api/Admin/AssignStudentToClass` - Assign student to class

### Teacher Endpoints
- `GET /api/Teacher/GetAll` - Get all teachers
- `POST /api/Teacher/Create` - Create new teacher
- `GET /api/Teacher/GetClasses/{teacherId}` - Get teacher's classes
- `GET /api/Teacher/GetSubjects/{teacherId}` - Get teacher's subjects

### Student Endpoints
- `GET /api/Student/GetAll` - Get all students
- `POST /api/Student/Create` - Create new student
- `GET /api/Student/GetClasses/{studentId}` - Get student's classes
- `POST /api/Student/EnrollClass/{classId}` - Enroll student in class

### Academic Management
- `GET /api/Subject/GetAll` - Get all subjects
- `POST /api/Subject/Create` - Create new subject
- `GET /api/Lesson/GetAll` - Get all lessons
- `POST /api/Lesson/Create` - Create new lesson

### Assessment System
- `POST /api/Quiz/CreateQuiz` - Create new quiz
- `GET /api/Quiz/GetQuizByCode/{quizCode}` - Get quiz details
- `POST /api/Quiz/SubmitQuizAttempt` - Submit quiz answers
- `GET /api/Quiz/GetQuizResults/{quizCode}` - Get quiz results

## 🔐 Authentication

The system uses JWT-based authentication with the following flow:

1. **Login**: Send credentials to `/api/Auth/Login`
2. **Token Response**: Receive access token and refresh token
3. **API Calls**: Include `Authorization: Bearer {access_token}` header
4. **Token Refresh**: Use refresh token to get new access token when expired

### User Roles
- **Admin**: Full system access, user management, analytics
- **Teacher**: Class management, quiz creation, student grading
- **Student**: View classes, take quizzes, view grades

## 🗄️ Database Schema

### Core Entities
- **Users**: Base user entity with Identity integration
- **Students**: Extended user with academic information
- **Teachers**: Extended user with teaching assignments
- **Grades**: Academic grade levels
- **Classes**: Class entities with grade associations
- **Subjects**: Academic subjects with teacher assignments
- **Lessons**: Individual lessons within subjects
- **Quizzes**: Assessment quizzes linked to lessons
- **Questions**: Quiz questions with multiple types
- **Answers**: Student quiz responses

### Key Relationships
- Many-to-Many: Students ↔ Classes, Teachers ↔ Classes
- One-to-Many: Grade → Classes, Subject → Lessons
- Many-to-Many: Students ↔ Subjects, Students ↔ Quizzes

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Contact

Email[mmmelkady23@gmail.com]

---

⭐ If you found this project helpful, please give it a star!

