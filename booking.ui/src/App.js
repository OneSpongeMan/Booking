import logo from './logo.svg';
import './App.css';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Table from './Components/Tables/Table';
import Guest from './Components/Guests/Guest';
import Reservation from './Components/Reservations/Reservation';
import NavigationMenu from './Components/Menus/NavigationMenu';

// const Guest = () => <div style={{ padding: '20px' }}><h2>Список гостей</h2><p>Данные загружаются с /api/guests...</p></div>;
// const Reservation = () => <div style={{ padding: '20px' }}><h2>Бронирования</h2><p>Данные загружаются с /api/reservations...</p></div>;

function App() {
  return (
    <div className="App">
      <BrowserRouter>
            {/* Меню находится ВНЕ блока Routes, отображается всегда */}
            <NavigationMenu />
            
            <div className="main-content">
                <Routes>
                    {/* Если пользователь зашел на главную (/) перенаправляем на столики */}
                    <Route path="/" element={<Navigate to="/tables" replace />} />
                    
                    <Route path="/tables" element={<Table />} />
                    <Route path="/guests" element={<Guest />} />
                    <Route path="/reservations" element={<Reservation />} />
                    
                    {/* Обработка несуществующих ссылок */}
                    <Route path="*" element={<h2 style={{ padding: '20px' }}>Страница не найдена</h2>} />
                </Routes>
            </div>
        </BrowserRouter>
    </div>
  );
}

export default App;
