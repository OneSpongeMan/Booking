import React from 'react'
import {NavLink} from 'react-router-dom'
import '../../Styles/Navigation.css'

function NavigationMenu() {
    return (
        <nav className="navbar">
            <div className="navbar-brand">Система резервирования столиков</div>
            
            <div className="navbar-links">
                {/* 
                    to="..." - это путь в URL (например, http://localhost:3000/tables).
                    Функция className проверяет, активна ли ссылка, и применяет нужный стиль.
                */}
                <NavLink
                    to="/reservations" 
                    className={({ isActive }) => isActive ? "navbar-link active" : "navbar-link"}>
                    Бронирования
                </NavLink>

                <NavLink
                    to="/tables" 
                    className={({ isActive }) => isActive ? "navbar-link active" : "navbar-link"}>
                    Столики
                </NavLink>
                
                <NavLink
                    to="/guests" 
                    className={({ isActive }) => isActive ? "navbar-link active" : "navbar-link"}>
                    Гости
                </NavLink>
            </div>
        </nav>
    )
}

export default NavigationMenu