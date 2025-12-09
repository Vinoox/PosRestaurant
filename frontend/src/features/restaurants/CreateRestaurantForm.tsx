import { useState } from 'react';
import agent from '../../api/agent';

interface Props {
    onSuccess: () => void; // Po stworzeniu odświeżymy listę lub zalogujemy
    onCancel: () => void;
}

export default function CreateRestaurantForm({ onSuccess, onCancel }: Props) {
    const [name, setName] = useState('');

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            // POST /api/restaurants - wymaga tokena (Axios interceptor go doda)
            await agent.Restaurants.create({ name });
            alert("Restauracja została utworzona!");
            onSuccess();
        } catch (error) {
            console.error(error);
            alert("Nie udało się utworzyć restauracji.");
        }
    }

    return (
        <div style={{ maxWidth: '400px', margin: '20px auto', textAlign: 'center', border: '1px solid #ddd', padding: '20px', borderRadius: '8px' }}>
            <h3>Nowa Restauracja</h3>
            <p>Podaj nazwę swojego lokalu, aby rozpocząć.</p>
            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                <input 
                    value={name} 
                    onChange={e => setName(e.target.value)} 
                    placeholder="Nazwa restauracji" 
                    required 
                    style={{padding: '10px'}}
                />
                <button type="submit" style={{padding: '10px', background: '#007bff', color: 'white', border: 'none'}}>Utwórz</button>
                <button type="button" onClick={onCancel} style={{padding: '10px', background: '#6c757d', color: 'white', border: 'none'}}>Wróć</button>
            </form>
        </div>
    );
}