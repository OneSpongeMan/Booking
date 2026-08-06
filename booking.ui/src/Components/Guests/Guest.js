import React, {useState, useEffect} from 'react'
import api from '../API'
import GuestModal from './GuestModal'
import GuestCreate from './GuestCreate'
import GuestMenu from '../Menus/GuestMenu'

import '../../Styles/SimpleStyles.css'

function Guest() {
    const [guests, setGuests] = useState([]);
    const [guestForm, setGuestForm] = useState();
    const [guestCreate, setGuestCreate] = useState(false)

    const [filterParams, setFilterParams] = useState({})
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        let endpoint = '/Guest';
        const activeFilterKeys = Object.keys(filterParams);
        
        if (activeFilterKeys.length > 0) {
            const filterName = activeFilterKeys[0];
            const filterValue = filterParams[filterName];
            
            endpoint = `/Guest/${filterName}:${filterValue}`;
            if (filterName === 'regularCustomer')
                endpoint = '/Guest/regular'
        }

        api.get(endpoint)
            .then(response => {
                if (response.data.length > 0)
                    setGuests(response.data);
                else
                {
                    if (response.data.hasOwnProperty('length'))
                        setGuests(response.data)
                    else                    
                        setGuests([response.data])
                }
            })
            .catch(err => {
                const message = err.response.data.error || 'Ошибка загрузки посетителей';
                setError(message);
            })
            .finally(() => {
                setLoading(false);
            });
    }, [filterParams, guestCreate, guestForm]);

    if (loading) return <div>Загрузка списка посетителей...</div>;

    const HandleGuestClick = (guest) => {
        setGuestForm({
            id: guest.id, 
            name: guest.name, 
            surname: guest.surname, 
            patronymic: guest.patronymic, 
            phone: guest.phone, 
            regularCustomer: guest.regularCustomer
        })
    };
    
    const CloseGuestDetails = () => {
        setGuestForm(null);
        setGuestCreate(false);
    };

    const UpdateGuestDetails = (guest) => {
        api.put('/Guest', guest)
            .then(() => {
                CloseGuestDetails();
            })
            .catch(err => {
                console.error(err);
                alert("Не удалось обновить информацию о госте!");
            });
    };

    const DeleteGuestDetails = (guestId) => {
        if (!window.confirm("Вы уверены, что хотите удалить этого гостя?")) return;

        api.delete(`/Guest/${guestId}`)
            .then(() => {
                CloseGuestDetails();
            })
            .catch(err => {
                console.error(err);
                alert("Не удалось удалить гостя!");
            });
    };

    const HandleGuestCreate = () => {
        setGuestCreate(true);
    }

    const CreateGuestDetails = (guest) => {
        api.post('/Guest', guest)
            .then((response) => {
                CloseGuestDetails();
            })
            .catch(err => {
                console.error(err);
                alert("Не удалось создать гостя!");
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
        <h2>Список гостей</h2>
            <div>
            {
                guests.length ?
                guests.map(guest =>
                    <div 
                        key={guest.id} 
                        onClick={() => HandleGuestClick(guest)}
                        className='simple-content'>
                        {guest.surname && guest.surname} {guest.name}, {guest.phone}
                    </div>
                ) : null
            }
            {
                error ? <div>{error}</div> : null
            }
            {
                guestForm && (
                    <GuestModal details={guestForm} 
                                onClose={CloseGuestDetails}
                                onUpdate={UpdateGuestDetails}
                                onDelete={DeleteGuestDetails}/>
                )
            }
            {
                <GuestMenu 
                    onApplyFilters={HandleApplyFilters}
                    onResetFilters={HandleResetFilters}
                />
            }
            {
                guestCreate && (
                    <GuestCreate 
                        onCreate={CreateGuestDetails}
                        onClose={CloseGuestDetails}/>
                )
            }          
            </div>
            <button className='simple-btn-create' onClick={HandleGuestCreate}>Создать гостя</button>
        {
            error && (
                HandleError()                
            )
        }
        </div>
    )
}

export default Guest