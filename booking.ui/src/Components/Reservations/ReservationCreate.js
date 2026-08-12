import React, {useState} from 'react'
import '../../Styles/SimpleStyles.css'

function ReservationCreate({onCreate, onClose}) {
    const [reservation, setReservation] = useState({table: '', guest: '', start: '', end: '', personsNumber: '', tempBooked: false, comment: ''});
    
    const HandleSubmit = (e) => {
        e.preventDefault();
        onCreate(reservation);
    }

    if (!reservation) return null;

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                <div className="modal-header">
                    <h3>Заполните информацию о бронировании</h3>
                    <button type="button" className="modal-btn-close" onClick={onClose}>x</button>
                </div>

                <form onSubmit={HandleSubmit}>
                    <div className='modal-input-group'>
                        <label>Номер столика: </label>
                        <input 
                            type='number' 
                            value={reservation.table} 
                            placeholder='5'
                            onChange={(e) => setReservation({ ...reservation, table: e.target.value })}
                            required
                        />
                    </div>

                    <div className='modal-input-group'>
                        <label>ID гостя: </label>
                        <input 
                            type='text' 
                            value={reservation.guest} 
                            placeholder='Например, af12e24d-0b46-4b65-bf73-b9fbc8e3a047'
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
                            placeholder='Например, 2'
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
                        <button type="submit" className="modal-btn-save">Создать</button>
                        <button type="button" className="modal-btn-cancel" onClick={onClose}>Отменить</button>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default ReservationCreate