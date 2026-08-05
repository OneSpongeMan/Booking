import React, {useState} from 'react'
import '../../Styles/Navigation.css'

function GuestMenu({onApplyFilters, onResetFilters}) {
    const initialFilterState = {
        id: '',
        phone: '',
        fullname: '',
        regularCustomer: ''
    };

    const [filters, setFilters] = useState(initialFilterState);

    const HandleChange = (e) => {
        const { name, value } = e.target;
        setFilters(prev => ({ 
            ...initialFilterState, 
            [name]: value 
        }));
    };

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
            <h3>Фильтры посетителей</h3>
            
            <form onSubmit={HandleSubmit}>
                <div className='sidebar-filter-group'>
                    <label htmlFor='id'>ID гостя:</label>
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
                    <label htmlFor='fullname'>Полное имя:</label>
                    <input
                        type='text'
                        id='fullname'
                        name='fullname'
                        placeholder='Например, Иванов Иван Иванович'
                        value={filters.fullname}
                        onChange={HandleChange}
                    />
                </div>

                <div className='sidebar-filter-group'>
                    <label htmlFor='phone'>Номер телефона:</label>
                    <input
                        type='text'
                        id='phone'
                        name='phone'
                        placeholder='Например, +11111111111'
                        value={filters.phone}
                        onChange={HandleChange}
                    />
                </div>

                <div className='sidebar-filter-group'>
                    <label htmlFor='regular'>Постоянные клиенты:</label>
                    <select
                        id='regular'
                        name='regular'
                        value={filters.regularCustomer}
                        onChange={HandleChange}
                    >
                        <option value=''>Все клиенты</option>
                        <option value='true'>Только постоянные клиенты</option>
                    </select>
                </div>

                <div className='sidebar-filter-actions'>
                    <button type='submit' className='sidebar-btn-apply'>Применить</button>
                    <button type='button' className='sidebar-btn-reset' onClick={HandleReset}>Сбросить</button>
                </div>
            </form>
        </aside>
    )
}

export default GuestMenu