import React, {useState, useEffect} from 'react'
import '../../Styles/SimpleStyles.css'

function GuestModal({details, onClose, onUpdate, onDelete}) {
    const [guest, setGuest] = useState({id: '', name: '', surname: '', patronymic: '', phone: '', regularCustomer: ''});
    
    useEffect(() => {
        setGuest(details)
    }, [details]);

    const handleSubmit = (e) => {
        e.preventDefault();
        onUpdate(guest);
    }

    if (!guest) return null;

    return (
        <div className='modal-overlay' onClick={onClose}>
            <div className='modal-content' onClick={(e) => e.stopPropagation()}>
                <div className='modal-header'>
                    <h3>Детальная информация о госте</h3>
                    <button type='button' className='modal-btn-close' onClick={onClose}>x</button>
                </div>

                <p><strong>ID: </strong>{guest.id}</p>

                <form onSubmit={handleSubmit}>
                    <div className='modal-input-group'>
                        <label>Имя гостя: </label>
                        <input 
                            type='text' 
                            value={guest.name} 
                            onChange={(e) => setGuest({ ...guest, name: e.target.value })}
                            required
                        />
                    </div>

                    <div className='modal-input-group'>
                        <label>Фамилия гостя: </label>
                        <input 
                            type='text' 
                            value={guest.surname} 
                            placeholder='Например, Иванов'
                            onChange={(e) => setGuest({ ...guest, surname: e.target.value })}
                        />
                    </div>

                    <div className='modal-input-group'>
                        <label>Отчество гостя: </label>
                        <input 
                            type='text' 
                            value={guest.patronymic} 
                            placeholder='Например, Иванович'
                            onChange={(e) => setGuest({ ...guest, patronymic: e.target.value })}
                        />
                    </div>

                    <div className='modal-input-group'>
                        <label>Номер телефона гостя: </label>
                        <input 
                            type='text' 
                            value={guest.phone} 
                            onChange={(e) => setGuest({ ...guest, phone: e.target.value })}
                        />
                    </div>

                    <div className='modal-input-group-checkbox'>
                        <label>Является постоянным клиентом?</label>
                        <input 
                            type='checkbox'
                            checked={guest.regularCustomer} 
                            onChange={(e) => setGuest({ ...guest, regularCustomer: e.target.checked })}
                        />
                    </div>

                    <div className='modal-actions'>
                        <button type='submit' className='modal-btn-save'>Обновить</button>
                        <button type='button' className='modal-btn-cancel' onClick={() => onDelete(guest.id)}>Удалить</button>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default GuestModal