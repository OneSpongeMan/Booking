import React, {useState, useEffect} from 'react'
import api from '../API'
import ReservationModal from './ReservationModal';
import ReservationCreate from './ReservationCreate';
import ReservationMenu from '../Menus/ReservationMenu';

import '../../Styles/SimpleStyles.css'
import FormatToReadableDate from '../ISOConverter'


function Reservation() {
    const [reservations, setReservations] = useState([]);
    const [reservationForm, setReservationForm] = useState();
    const [reservationCreate, setReservationCreate] = useState(false);
    const [createInitialData, setCreateInitialData] = useState(null);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [filterParams, setFilterParams] = useState({});    

    useEffect(() => {
        let endpoint = '/Reservation';
        const activeFilterKeys = Object.keys(filterParams);
        
        if (activeFilterKeys.length > 0) {
            const filterName = activeFilterKeys[0];
            const filterValue = filterParams[filterName];

            // Бэк изменился, другие требования к вызову API
            // Придумать более изящный способ смены рута
            if (filterName === 'start' || filterName === 'end')
                endpoint += `/available_tables/start=${filterParams.start}&end=${filterParams.end}`;
            else
                endpoint += `/${filterName}:${filterValue}`;
        }

        api.get(endpoint)
            .then(response => {
                if (response.data.length > 0)
                    setReservations(response.data);
                else {
                    if (response.data.hasOwnProperty('length'))
                        alert('Бронирования с такими параметрами не найдены');
                    else
                        setReservations([response.data])
                }
            })
            .catch(err => {
                const message = err.response.data.error || 'Ошибка загрузки бронирований';
                setError(message);
            })
            .finally(() => {
                setLoading(false);
            });
    }, [filterParams, reservationForm, reservationCreate]);

    if (loading) return <div>Загрузка списка бронирований...</div>;

    const HandleError = () => {
        alert(`Возникла проблема: ${error}`);
        setError(null);
    }

    const HandleApplyFilters = (newFilters) => {
        setFilterParams(newFilters);
    };

    const HandleResetFilters = () => {
        setFilterParams({});
    };

    const HandleItemClick = (item) => {
        if (item.seats !== undefined) {
            setCreateInitialData({
                table: item.number,
                start: filterParams.start || '',
                end: filterParams.end || '',
                guest: '',
                personsNumber: '',
                tempBooked: false,
                comment: ''
            });
            setReservationCreate(true);

        } else {
            setReservationForm({
                id: item.id,
                table: item.table,
                start: item.start,
                end: item.end,
                guest: item.guest,
                personsNumber: item.personsNumber,
                tempBooked: item.tempBooked,
                comment: item.comment
            })
        }
    }

    const CloseReservationDetails = () => {
        // Посмотреть сброс фильтров в других частях фронта
        // setFilterParams({});
        setReservationForm();
        setReservationCreate(false);
        setCreateInitialData(null);
    }

    const HandleReservationCreate = () => {
        setReservationCreate(true);
    }

    const CreateReservation = (reservation) => {
        api.post('/Reservation', reservation)
            .then((response) => {
                CloseReservationDetails();
            })
            .catch(err => {
                alert(`Не удалось забронировать стол: ${err.response.data.error}`);
            });
    }

    const UpdateReservation = (reservation) => {
        api.put('/Reservation', reservation)
            .then((response) => {
                CloseReservationDetails();
            })
            .catch((err) => {
                alert(`Не удалось обновить бронь: ${err.response.data.error}`);
            })
    }

    const DeleteReservation = (reservationId) => {
        if (!window.confirm("Вы уверены, что хотите удалить эту бронь?")) return;

        api.delete(`/Reservation/${reservationId}`)
            .then((response) => {
                CloseReservationDetails();
            })
            .catch((err) => {
                alert(`Не удалось удалить бронь: ${err.response.data.error}`);
            })
    }
    
    return (
        <div className='simple-body'>
        <h2>Список бронирований</h2>
        <div>
            {
                error && (
                    HandleError()
                )
            }
            {
                reservations.length ?
                reservations.map(item =>
                    <div 
                        key={item.id}
                        onClick={() => HandleItemClick(item)}
                        className='simple-content'>
                        {/* Стол {reservation.table} <br /> {reservation.start}-{reservation.surname} */}
                        {item.seats !== undefined ? (
                            <span>Свободный столик №{item.number} ({item.seats} мест)</span>
                        ) : (
                            <span>Стол {item.table} <br /> {FormatToReadableDate(item.start)} - {FormatToReadableDate(item.end)}</span>
                        )}
                    </div>
                ) : null
            }
            {
                reservationForm && (
                    <ReservationModal details={reservationForm}
                                        onClose={CloseReservationDetails}
                                        onUpdate={UpdateReservation}
                                        onDelete={DeleteReservation}/>
                )
            }
            {
                reservationCreate && (
                    <ReservationCreate initialData={createInitialData}
                                        onCreate={CreateReservation}
                                        onClose={CloseReservationDetails}/>
                )
            }
            {
                <ReservationMenu 
                    onApplyFilters={HandleApplyFilters}
                    onResetFilters={HandleResetFilters}
                />
            }
            {
                <button className='simple-btn-create' onClick={HandleReservationCreate}>Создать бронирование</button>
            }
        </div>
        </div>
    )
}

export default Reservation