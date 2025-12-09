import { useState } from 'react';
import agent from '../../api/agent';

interface Props {
    onSuccess: () => void;
    onCancel: () => void;
}

export default function RegisterForm({ onSuccess, onCancel }: Props) {
    const [values, setValues] = useState({
        firstName: '',
        lastName: '',
        email: '',
        password: '',
        confirmPassword: '',
        pin: '' // <-- Dodajemy wymagany PIN
    });
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        // Prosta walidacja haseł
        if(values.password !== values.confirmPassword) {
            setError("Hasła muszą być identyczne");
            return;
        }

        // Walidacja PIN (musi być, bo backend tego wymaga)
        if(values.pin.length < 4) {
            setError("PIN musi mieć minimum 4 cyfry");
            return;
        }

        try {
            // Wysyłamy obiekt zgodny z RegisterUserDto w C#
            await agent.Auth.register({ 
                firstName: values.firstName,
                lastName: values.lastName,
                email: values.email,
                password: values.password,
                confirmPassword: values.confirmPassword,
                pin: values.pin
            });
            
            alert("Konto utworzone! Możesz się zalogować.");
            onSuccess();
        } catch (err: any) {
            console.error(err);
            // Wyciągamy błędy z API (często jest to tablica errors)
            if (err.response?.data?.errors) {
                const errorMessages = Object.values(err.response.data.errors).flat().join(', ');
                setError(errorMessages);
            } else {
                setError(err.response?.data?.title || "Błąd rejestracji. Sprawdź dane.");
            }
        }
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setValues({...values, [e.target.name]: e.target.value});
    }


    const inputStyle = { padding: '10px', borderRadius: '4px', border: '1px solid #ccc' };

    return (
        <div style={{ maxWidth: '400px', margin: 'auto', textAlign: 'center', padding: '20px' }}>
            <h2>Rejestracja</h2>
            
            {error && (
                <div style={{ background: '#ffebee', color: '#c62828', padding: '10px', marginBottom: '15px', borderRadius: '4px' }}>
                    {error}
                </div>
            )}

            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                <div style={{ display: 'flex', gap: '10px' }}>
                    <input 
                        name="firstName" 
                        placeholder="Imię" 
                        onChange={handleChange} 
                        required 
                        style={{...inputStyle, width: '100%'}}
                    />
                    <input 
                        name="lastName" 
                        placeholder="Nazwisko" 
                        onChange={handleChange} 
                        required 
                        style={{...inputStyle, width: '100%'}}
                    />
                </div>

                <input name="email" type="email" placeholder="Email" onChange={handleChange} required style={inputStyle}/>
                
                <input 
                    name="pin" 
                    type="text" 
                    placeholder="Kod PIN (np. 1234) - do logowania w lokalu" 
                    onChange={handleChange} 
                    required 
                    maxLength={4}
                    style={inputStyle}
                />

                <input name="password" type="password" placeholder="Hasło" onChange={handleChange} required style={inputStyle}/>
                <input name="confirmPassword" type="password" placeholder="Powtórz hasło" onChange={handleChange} required style={inputStyle}/>
                
                <button type="submit" style={{padding: '12px', background: '#28a745', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer', fontWeight: 'bold', marginTop: '10px'}}>
                    Utwórz konto
                </button>
                <button type="button" onClick={onCancel} style={{padding: '10px', background: 'transparent', border: 'none', cursor: 'pointer', color: '#666'}}>
                    Wróć do logowania
                </button>
            </form>
        </div>
    );
}