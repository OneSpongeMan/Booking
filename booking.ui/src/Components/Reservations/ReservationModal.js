import React, {useState, useEffect} from 'react'
import '../../Styles/SimpleStyles.css'

function ReservationModal({details, onClose, onUpdate, onDelete}) {
    const [reservation, setReservation] = useState({id: '', table: '', guest: '', start: '', end: '', personsNumber: '', tempBooked: false, comment: ''});
    
    useEffect(() => {
        setReservation(details)
    }, [details]);

    const handleSubmit = (e) => {
        e.preventDefault();
        onUpdate(reservation);
    }

    if (!reservation) return null;

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                <div className="modal-header">
                    <h3>Детальная информация о бронировании</h3>
                    <button type="button" className="modal-btn-close" onClick={onClose}>x</button>
                </div>

                <p><strong>ID: </strong>{reservation.id}</p>

                <form onSubmit={handleSubmit}>
                    <div className='modal-input-group'>
                        <label>Номер столика: </label>
                        <input 
                            type='number' 
                            value={reservation.table} 
                            onChange={(e) => setReservation({ ...reservation, table: e.target.value })}
                            required
                        />
                    </div>

                    <div className='modal-input-group'>
                        <label>ID гостя: </label>
                        <input 
                            type='text' 
                            value={reservation.guest} 
                            onChange={(e) => setReservation({ ...reservation, guest: e.target.value })}
                            required
                        />
                    </div>

                    <div className='modal-input-group'>
                        <label>Дата/время начала бронирования: </label>
                        <input 
                            type='datetime-local' 
                            value={reservation.start} 
                            onChange={(e) => setReservation({ ...reservation, start: e.target.value })}
                            required
                        />
                    </div>

                    <div className='modal-input-group'>
                        <label>Дата/время окончания бронирования: </label>
                        <input 
                            type='datetime-local' 
                            value={reservation.end} 
                            onChange={(e) => setReservation({ ...reservation, end: e.target.value })}
                            required
                        />
                    </div>

                    <div className='modal-input-group'>
                        <label>Число персон: </label>
                        <input 
                            type='number' 
                            value={reservation.personsNumber} 
                            onChange={(e) => setReservation({ ...reservation, personsNumber: e.target.value })}
                            required
                        />
                    </div>

                    <div className="modal-input-group-checkbox">
                        <label>Временное бронирование?</label>
                        <input 
                            type="checkbox"
                            checked={reservation.tempBooked} 
                            onChange={(e) => setReservation({ ...reservation, tempBooked: e.target.checked })}
                        />
                    </div>

                    <div className='modal-input-group'>
                        <label>Пожелания клиента: </label>
                        <textarea 
                            value={reservation.comment} 
                            onChange={(e) => setReservation({ ...reservation, comment: e.target.value })}
                        />
                    </div>

                    <div className="modal-actions">
                        <button type="submit" className="modal-btn-save">Обновить</button>
                        <button type="button" className="modal-btn-cancel" onClick={() => onDelete(reservation.id)}>Удалить</button>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default ReservationModal