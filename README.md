# Twitter Clone Project 🐦

[![LinkedIn](https://img.shields.io/badge/LinkedIn-Connect-blue)](https://www.linkedin.com/posts/abanoub-saweris_dotnet-activity-7251686920122974209-MX_g?utm_source=share&utm_medium=member_desktop)

This project is a feature-rich Twitter clone built using **ASP.NET Web API** and **Onion Architecture**. The application replicates many core functionalities of Twitter, including user authentication, posting, commenting, liking, and following. I used a cloud **PostgreSQL** database for managing user data and interactions.

## 🛠️ Technologies Used

- **ASP.NET Web API**
- **Onion Architecture**
- **PostgreSQL** (cloud database)
- **Mapster** (for object mapping)
- **FluentValidation** (for input validation)

## 🔒 Features

- **Authentication & Authorization**: Secure user login and registration system.
- **Post Management**: Users can create, update, and delete posts.
- **Comments & Reposts**: Users can comment on posts and repost content to their profile.
- **Likes & Follows with Toggle**: Users can like/unlike posts and follow/unfollow other users.
- **Search Functionality**: Find users and posts quickly.
- **Real-Time Notifications**: Notifications for follows, likes, comments, and reposts.
- **Scalable Cloud Database**: PostgreSQL used for data storage.

## 🚀 Future Improvements

- **Real-Time Chat**: Planning to add a chat feature for direct messaging between users.

## 📂 Project Structure

The project is divided into several layers based on Onion Architecture:

- **Twitter.Api**: Contains the project structure and controllers.
- **Twitter.Application**: Manages models, business services, and database interactions.
- **Twitter.Contract**: Includes all Data Transfer Objects (DTOs) used across the application.

## 🛡️ Security

The application implements secure user authentication and authorization mechanisms to protect user data.

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## 🤝 Connect

Feel free to check out my [LinkedIn post](https://www.linkedin.com/posts/abanoub-saweris_dotnet-activity-7251686920122974209-MX_g?utm_source=share&utm_medium=member_desktop) about the project or connect with me for any queries.
