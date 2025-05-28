# Fitness & Nutrition Tracker – .NET MAUI Android App

This is a cross-platform mobile app built with **.NET MAUI**, designed to help users monitor their daily meals, exercises, and hydration levels. The app connects to **Firebase** for data storage and uses external APIs to fetch nutrition and exercise details.

---

## 📱 Features

- 🔐 **User Authentication**
  - Register and log in with email and password via Firebase.

- 🏠 **Home Dashboard**
  - View today’s logged meals and exercise progress.
  - Progress bar shows percentage toward the user’s daily fitness goal.
  - Input for tracking water consumption.

- 🍽️ **Add Meal Page**
  - Enter meal type (e.g., breakfast, lunch, dinner).
  - Input ingredients and quantities.
  - Fetch detailed nutritional information from the [CalorieNinjas API](https://api.calorieninjas.com).

- 🏋️ **Add Exercise Page**
  - Type exercise name and select duration using a TimePicker.
  - Retrieves a list of matching exercises from [API Ninjas](https://api.api-ninjas.com).

- 📊 **History**
  - View full history of meals and exercises logged by the current user.

- 👤 **User Profile**
  - Set personalized goals for calories and exercise duration.

---

## 🧱 Architecture

- **Views**: UI pages (login, home, meals, exercises, history, profile)
- **Entities**: Data models for meals, exercises, and nutrition values
- **Services**:
  - `FirebaseService.cs` – Firebase data interactions
  - `NutritionService.cs` – Fetches food nutrition info
  - `ExerciseService.cs` – Retrieves exercise suggestions

---

## 🔧 Technologies Used

- [.NET MAUI](https://learn.microsoft.com/en-us/dotnet/maui/)
- Firebase Authentication & Realtime Database
- [CalorieNinjas API](https://calorieninjas.com/)
- [API Ninjas](https://api-ninjas.com/)
- C#, XAML, MVVM

---

## 📸 Screenshots

![image](https://github.com/user-attachments/assets/66ef2e0c-6672-4079-b2af-694224aeb383)
![image](https://github.com/user-attachments/assets/628fd757-dc0c-46cc-ad33-8616e986cc42)
![image](https://github.com/user-attachments/assets/42f434e0-bdc4-4249-8378-bab0fffa2097)
![image](https://github.com/user-attachments/assets/8996e08a-5fe5-4956-98aa-8be62dd8a29c)
![image](https://github.com/user-attachments/assets/cb7b560f-ca94-44c9-8fca-4d595124c357)

