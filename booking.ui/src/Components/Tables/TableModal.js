import React, {useState, useEffect} from 'react'
import '../../Styles/SimpleStyles.css'

function TableModal({details, onClose, onUpdate, onDelete}) {
    const [table, setTable] = useState({id: '', number: '', seats: '', nearFountain: ''});

    useEffect(() => {
        setTable(details)
    }, [details]);

    const handleSubmit = (e) => {
        e.preventDefault();
        onUpdate(table);
    }

    if (!table) return null;

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                <div className="modal-header">
                    <h3>Детальная информация о столике</h3>
                    <button type="button" className="modal-btn-close" onClick={onClose}>x</button>
                </div>

                <p><strong>ID: </strong>{table.id}</p>

                <form onSubmit={handleSubmit}>
                    <div className="modal-input-group">
                        <label>Номер столика: </label>
                        <input 
                            type="number" 
                            value={table.number} 
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
                        <button type="submit" className="modal-btn-save">Обновить</button>
                        <button type="button" className="modal-btn-cancel" onClick={() => onDelete(table.id)}>Удалить</button>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default TableModal