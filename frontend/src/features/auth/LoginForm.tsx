import { useState } from 'react';
import agent from '../../api/agent';
import { type User } from '../../models/user'; // Musimy zaimportować typ User

// Definiujemy kontrakt: Czego ten komponent potrzebuje z zewnątrz?
interface Props {
    onSuccess: (user: User) => void; // Funkcja, którą wywołamy po udanym logowaniu
}

export default function LoginForm({ onSuccess }: Props) {
    const [values, setValues] = useState({
        email: 'admin@gmail.com',
        password: 'Admin123-'
    });
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        try {
            // 1. Wysyłamy żądanie do API
            const user = await agent.Auth.login(values);
            console.log("Zalogowano:", user);
            
            // 2. Zamiast wyświetlać alert, przekazujemy dane "w górę" do App.tsx
            onSuccess(user);
            
        } catch (err: any) {
            console.error(err);
            setError("Błąd logowania. Sprawdź email i hasło.");
        }
    }

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setValues({ ...values, [name]: value });
    }

    return (
        <div style={{ maxWidth: '400px', margin: 'auto', padding: '20px', border: '1px solid #eee', borderRadius: '8px', boxShadow: '0 2px 5px rgba(0,0,0,0.1)' }}>
            <h2 style={{color: '#333'}}>Zaloguj się do POS</h2>
            
            {error && (
                <div style={{ background: '#ffebee', color: '#c62828', padding: '10px', borderRadius: '4px', marginBottom: '15px' }}>
                    {error}
                </div>
            )}

            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '15px' }}>
                <input 
                    name="email" 
                    placeholder="Email" 
                    value={values.email} 
                    onChange={handleInputChange}
                    style={{ padding: '10px', fontSize: '16px', borderRadius: '4px', border: '1px solid #ccc' }}
                />
                <input 
                    name="password" 
                    type="password" 
                    placeholder="Hasło" 
                    value={values.password} 
                    onChange={handleInputChange} 
                    style={{ padding: '10px', fontSize: '16px', borderRadius: '4px', border: '1px solid #ccc' }}
                />
                <button 
                    type="submit" 
                    style={{ padding: '12px', background: '#007bff', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', fontSize: '16px', fontWeight: 'bold' }}
                >
                    Zaloguj
                </button>
            </form>
        </div>
    );
}