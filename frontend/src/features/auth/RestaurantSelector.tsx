import { type RestaurantSummary } from '../../models/user';

interface Props {
    restaurants: RestaurantSummary[];
    onSelect: (id: number) => void;
}

export default function RestaurantSelector({ restaurants, onSelect }: Props) {
    return (
        <div style={{ textAlign: 'center', marginTop: '20px' }}>
            <h2>Wybierz Restaurację</h2>
            <p>Twoje konto jest powiązane z wieloma lokalami.</p>
            
            <div style={{ display: 'flex', gap: '10px', justifyContent: 'center', flexWrap: 'wrap', marginTop: '20px' }}>
                {restaurants.map(r => (
                    <button 
                        key={r.id}
                        onClick={() => onSelect(r.id)}
                        style={{
                            padding: '20px',
                            minWidth: '150px',
                            background: '#fff',
                            border: '2px solid #007bff',
                            borderRadius: '8px',
                            cursor: 'pointer',
                            fontSize: '16px',
                            fontWeight: 'bold',
                            color: '#007bff'
                        }}
                    >
                        {r.name}
                    </button>
                ))}
            </div>
        </div>
    );
}