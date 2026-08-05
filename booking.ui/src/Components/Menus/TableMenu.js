import React, {useState} from 'react'
import '../../Styles/Navigation.css'


function TableMenu({onApplyFilters, onResetFilters}) {
    const initialFilterState = {
        number: '',
        seats: '',
        nearFountain: ''
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
            <h3>Фильтры столиков</h3>
            
            <form onSubmit={HandleSubmit}>
                <div className='sidebar-filter-group'>
                    <label htmlFor='number'>Номер столика:</label>
                    <input
                        type='number'
                        id='number'
                        name='number'
                        placeholder='Например, 12'
                        value={filters.number}
                        onChange={HandleChange}
                    />
                </div>

                <div className='sidebar-filter-group'>
                    <label htmlFor='seats'>Количество мест:</label>
                    <input
                        type='number'
                        id='seats'
                        name='seats'
                        placeholder='Например, 4'
                        value={filters.seats}
                        onChange={HandleChange}
                    />
                </div>

                <div className='sidebar-filter-group'>
                    <label htmlFor='nearFountain'>Расположение у фонтана:</label>
                    <select
                        id='nearFountain'
                        name='nearFountain'
                        value={filters.nearFountain}
                        onChange={HandleChange}
                    >
                        <option value=''>Все столики</option>
                        <option value='true'>Только у фонтана</option>
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

export default TableMenu