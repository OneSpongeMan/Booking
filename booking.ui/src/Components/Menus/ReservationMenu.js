import React, {useState} from 'react'
import '../../Styles/Navigation.css'

function ReservationMenu({onApplyFilters, onResetFilters}) {
    const initialFilterState = {
            id: '',
            guestId: '',
            table: '',
            start: '',
            end: '',
        };
    
        const [filters, setFilters] = useState(initialFilterState);
    
        const HandleChange = (e) => {
            const { name, value } = e.target;
            setFilters(prev => ({ 
                ...initialFilterState,
                [name]: value 
            }));
        };
        
        // При обновлении временных интервалов фильтрации должны сбрасываться все параметры, кроме их самих
        const HandleChangeTime = (e) => {
            const { name, value } = e.target;
            setFilters(prev => ({
                ...initialFilterState,
                start: prev.start,
                end: prev.end,
                [name]: value 
            }));
        }
    
        const HandleSubmit = (e) => {
            e.preventDefault();
    
            const activeFilters = {};
            Object.keys(filters).forEach(key => {
                if (filters[key] !== '') {
                    activeFilters[key] = filters[key];
                }
            });
    
            onApplyFilters(activeFilters);
        };
    
        const HandleReset = () => {
            setFilters(initialFilterState);
            onResetFilters();
        };
    
        return (
            <aside className='sidebar-filter'>
                <h3>Фильтры бронирований</h3>
                
                <form onSubmit={HandleSubmit}>
                    <div className='sidebar-filter-group'>
                        <label htmlFor='id'>ID брони:</label>
                        <input
                            type='text'
                            id='id'
                            name='id'
                            placeholder='Например, af12e24d-0b46-4b65-bf73-b9fbc8e3a047'
                            value={filters.id}
                            onChange={HandleChange}
                        />
                    </div>

                    <div className='sidebar-filter-group'>
                        <label htmlFor='guestId'>ID гостя:</label>
                        <input
                            type='text'
                            id='guestId'
                            name='guestId'
                            placeholder='Например, b9fbc8e3-0b46-4b65-bf73-af12e24da047'
                            value={filters.guestId}
                            onChange={HandleChange}
                        />
                    </div>

                    <div className='sidebar-filter-group'>
                        <label htmlFor='table'>Номер столика:</label>
                        <input
                            type='text'
                            id='table'
                            name='table'
                            placeholder='Например, 5'
                            value={filters.table}
                            onChange={HandleChange}
                        />
                    </div>

                    <div className='sidebar-filter-group'>
                        <label htmlFor='start'>Начало брони:</label>
                        <input
                            type='datetime-local'
                            id='start'
                            name='start'
                            value={filters.start}
                            onChange={HandleChangeTime}
                        />
                    </div>

                    <div className='sidebar-filter-group'>
                        <label htmlFor='end'>Окончание брони:</label>
                        <input
                            type='datetime-local'
                            id='end'
                            name='end'
                            value={filters.end}
                            onChange={HandleChangeTime}
                        />
                    </div>
    
                    <div className='sidebar-filter-actions'>
                        <button type='submit' className='sidebar-btn-apply'>Применить</button>
                        <button type='button' className='sidebar-btn-reset' onClick={HandleReset}>Сбросить</button>
                    </div>
                </form>
            </aside>
        )
}

export default ReservationMenu