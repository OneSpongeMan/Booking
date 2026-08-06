import React, {useState, useEffect} from 'react'
import api from '../API'
import TableModal from './TableModal';
import TableMenu from '../Menus/TableMenu';
import TableCreate from './TableCreate';

import '../../Styles/SimpleStyles.css'

function Table() {
    const [tables, setTables] = useState([]);
    // Возможно лишнее присвоение, но оставить на случай бугурта реакта
    // const [tableForm, setTableForm] = useState({id: '', number: '', seats: '', nearFountain: ''});
    const [tableForm, setTableForm] = useState();
    const [tableCreate, setTableCreate] = useState(false)

    const [filterParams, setFilterParams] = useState({}) // Параметры фильтрации столиков, одновременно только один
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        let endpoint = '/Table';
        const activeFilterKeys = Object.keys(filterParams);
        
        // На будущее - покопаться с контроллерами на бэке
        if (activeFilterKeys.length > 0) {
            const filterName = activeFilterKeys[0];
            const filterValue = filterParams[filterName];
            
            endpoint = `/Table/${filterName}:${filterValue}`;
            if (filterName === 'nearFountain')
                endpoint = '/Table/fountain'
        }

        api.get(endpoint)
            .then(response => {
                if (response.data.length > 0)
                    setTables(response.data.sort((a, b) => a.number - b.number));
                // Проблема с ответом в виде массива, если он не содержит элементов
                // Рисуется единственный пустой блок
                // Придумать решение получше
                else
                {
                    if (response.data.hasOwnProperty('length'))
                        setTables(response.data)
                    else                    
                        setTables([response.data])
                }
            })
            .catch(err => {
                const message = err.response.data.error || 'Ошибка загрузки столиков';
                setError(message);
            })
            .finally(() => {
                setLoading(false);
            });
    }, [filterParams, tableCreate, tableForm]);

    if (loading) return <div>Загрузка столиков...</div>;

    const HandleTableClick = (table) => {
        setTableForm({id: table.id, number: table.number, seats: table.seats, nearFountain: table.nearFountain})
    };
    
    const CloseTableDetails = () => {
        setTableForm(null);
        setTableCreate(false);
    };

    const UpdateTableDetails = (table) => {
        api.put('/Table', table)
            .then(() => {
                // Обновляем локальный массив: обновляем столик
                // setTables(tables.map(t => t.id === table.id ? table : t));
                CloseTableDetails();
            })
            .catch(err => {
                console.error(err);
                alert("Не удалось обновить столик!");
            });
    };

    const DeleteTableDetails = (tableId) => {
        if (!window.confirm("Вы уверены, что хотите удалить этот столик?")) return;

        api.delete(`/Table/${tableId}`)
            .then(() => {
                // Обновляем локальный массив: выкидываем удаленный столик
                // setTables(tables.filter(t => t.id !== tableId));
                CloseTableDetails();
            })
            .catch(err => {
                console.error(err);
                alert("Не удалось удалить столик!");
            });
    };

    const HandleTableCreate = () => {
        setTableCreate(true);
    }

    const CreateTableDetails = (table) => {
        api.post('/Table', table)
            .then((response) => {
                CloseTableDetails();
            })
            .catch(err => {
                console.error(err);
                alert("Не удалось создать столик!");
            });
    }

    const HandleApplyFilters = (newFilters) => {
        setFilterParams(newFilters);
    };

    const HandleResetFilters = () => {
        setFilterParams({});
    };

    const HandleError = () => {
        alert(`Возникла проблема: ${error}`);
        setError(null);
    }

    return (
        <div className='simple-body'>
        <h2>Столики в зале</h2>
            <div>
            {
                tables.length ?
                tables.map(table =>
                    <div 
                        key={table.id} 
                        onClick={() => HandleTableClick(table)}
                        className='simple-content'>
                        Столик №{table.number}
                    </div>
                ) : null
            }
            {
                tableForm && (
                    <TableModal details={tableForm} 
                                onClose={CloseTableDetails}
                                onUpdate={UpdateTableDetails}
                                onDelete={DeleteTableDetails}/>
                )
            }
            {
                <TableMenu 
                    onApplyFilters={HandleApplyFilters}
                    onResetFilters={HandleResetFilters}
                />
            }
            {
                tableCreate && (
                    <TableCreate 
                        onCreate={CreateTableDetails}
                        onClose={CloseTableDetails}/>
                )
            }
            </div>
            <button className='simple-btn-create' onClick={HandleTableCreate}>Создать столик</button>
        {
            error && (
                HandleError()                
            )
        }
        </div>
    )
}

export default Table