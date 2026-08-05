import React, {useState, useEffect} from 'react'
import '../../Styles/SimpleStyles.css'

function TableCreate({onCreate, onClose}) {
    const [table, setTable] = useState({number: '', seats: '', nearFountain: false});

    const HandleSubmit = (e) => {
        e.preventDefault();
        onCreate(table);
    }

    if (!table) return null;

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                <div className="modal-header">
                    <h3>Заполните информацию о столике</h3>
                    <button type="button" className="modal-btn-close" onClick={onClose}>x</button>
                </div>

                <form onSubmit={HandleSubmit}>
                    <div className="modal-input-group">
                        <label>Номер столика: </label>
                        <input 
                            type="number" 
                            value={table.number} 
                            placeholder='Например, 1'
                            min={1}
                            onChange={(e) => setTable({ ...table, number: e.target.value })}
                            required
                        />
                    </div>

                    <div className="modal-input-group">
                        <label>Количество мест: </label>
                        <input 
                            type="number" 
                            value={table.seats}
                            placeholder='Например, 3'
                            min={1}
                            onChange={(e) => setTable({ ...table, seats: e.target.value })}
                            required
                        />
                    </div>

                    <div className="modal-input-group-checkbox">
                        <label>Около фонтана?</label>
                        <input 
                            type="checkbox"
                            checked={table.nearFountain} 
                            onChange={(e) => setTable({ ...table, nearFountain: e.target.checked })}
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

export default TableCreate