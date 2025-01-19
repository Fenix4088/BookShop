/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './BookShop.Web/Views/**/*.cshtml',  // Указываем все представления MVC
    './BookShop.Web/wwwroot/**/*.html',  // Если используете статические HTML-файлы
    // './Pages/**/*.cshtml'   // Если используете Razor Pages
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}

