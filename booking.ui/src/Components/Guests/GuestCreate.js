import React, {useState} from 'react'
import '../../Styles/SimpleStyles.css'

function GuestCreate({onCreate, onClose}) {    
    const [guest, setGuest] = useState({name: '', surname: '', patronymic: '', phone: ''});

    const HandleSubmit = (e) => {
        e.preventDefault();
        onCreate(guest);
    }

    if (!guest) return null;

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                <div className="modal-header">
                    <h3>Заполните информацию о клиенте</h3>
                    <button type="button" className="modal-btn-close" onClick={onClose}>x</button>
                </div>

                <form onSubmit={HandleSubmit}>
                    <div className='modal-input-group'>
                        <label>Фамилия гостя: </label>
                        <input 
                            type='text' 
                            value={guest.surname} 
                            placeholder='Иванов'
                            onChange={(e) => setGuest({ ...guest, surname: e.target.value })}
                        />
                    </div>

                    <div className='modal-input-group'>
                        <label>Имя гостя: </label>
                        <input 
                            type='text' 
                            value={guest.name} 
                            placeholder='Например, Иван'
                            onChange={(e) => setGuest({ ...guest, name: e.target.value })}
                            required
                        />
                    </div>

                    <div className='modal-input-group'>
                        <label>Отчество гостя: </label>
                        <input 
                            type='text' 
                            value={guest.patronymic} 
                            placeholder='Иванович'
                            onChange={(e) => setGuest({ ...guest, patronymic: e.target.value })}
                        />
                    </div>

                    <div className='modal-input-group'>
                        <label>Номер телефона гостя: </label>
                        <input 
                            type='text' 
                            value={guest.phone} 
                            placeholder='Например, +11111111111'
                            onChange={(e) => setGuest({ ...guest, phone: e.target.value })}
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

export default GuestCreate