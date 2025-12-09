import { useState } from 'react';
import agent from './api/agent';
import { type User } from './models/user';


import LoginForm from './features/auth/LoginForm';
import RegisterForm from './features/auth/RegisterForm';
import RestaurantSelector from './features/auth/RestaurantSelector';
import CreateRestaurantForm from './features/restaurants/CreateRestaurantForm';

// Typy widoków, jakie mamy w aplikacji
type ViewState = 'LOGIN' | 'REGISTER' | 'SELECT_RESTAURANT' | 'CREATE_RESTAURANT' | 'DASHBOARD';

function App() {
  const [view, setView] = useState<ViewState>('LOGIN');
  const [user, setUser] = useState<User | null>(null);

  // --- LOGIKA PRZEJŚĆ (Transitions) ---

  // 1. Użytkownik się zalogował -> zapisujemy go i idziemy do wyboru restauracji
  const handleLoginSuccess = (userData: User) => {
      localStorage.setItem('jwt', userData.authenticationToken);
      setUser(userData);
      
      // Logika biznesowa: Jeśli nie ma restauracji, od razu proponujemy stworzenie
      if (userData.availableRestaurants.length === 0) {
          setView('CREATE_RESTAURANT');
      } else {
          setView('SELECT_RESTAURANT');
      }
  };

  // 2. Wybór restauracji -> pobieramy finalny token i idziemy do Dashboardu
  const handleRestaurantSelect = async (restaurantId: number) => {
      try {
          const response = await agent.Auth.selectRestaurant({ restaurantId });
          localStorage.setItem('jwt', response.token); // Nadpisujemy token
          setView('DASHBOARD');
      } catch (error) {
          alert("Błąd wejścia do restauracji");
      }
  };

  // 3. Sukces po stworzeniu restauracji -> Musimy odświeżyć usera (żeby widział nową restaurację)
  // Uproszczenie: Wylogowujemy go, żeby zalogował się ponownie i pobrał świeżą listę
  const handleCreateSuccess = () => {
      alert("Zaloguj się ponownie, aby zobaczyć nową restaurację.");
      setView('LOGIN');
      setUser(null);
      localStorage.removeItem('jwt');
  };

  // --- RENDEROWANIE WIDOKÓW ---

  if (view === 'DASHBOARD') {
      return (
          <div style={{textAlign: 'center', marginTop: 50}}>
              <h1>🚀 Pulpit Zarządzania (Dashboard)</h1>
              <p>Jesteś zalogowany w kontekście konkretnej restauracji.</p>
              <button onClick={() => { setView('LOGIN'); setUser(null); localStorage.removeItem('jwt'); }}>Wyloguj</button>
          </div>
      );
  }

  if (view === 'REGISTER') {
      return <RegisterForm onSuccess={() => setView('LOGIN')} onCancel={() => setView('LOGIN')} />;
  }

  if (view === 'CREATE_RESTAURANT') {
      return <CreateRestaurantForm onSuccess={handleCreateSuccess} onCancel={() => setView(user && user.availableRestaurants.length > 0 ? 'SELECT_RESTAURANT' : 'LOGIN')} />;
  }

  if (view === 'SELECT_RESTAURANT' && user) {
      return (
          <div>
              <RestaurantSelector restaurants={user.availableRestaurants} onSelect={handleRestaurantSelect} />
              <div style={{textAlign: 'center', marginTop: 20}}>
                  <p>Lub stwórz nowy lokal:</p>
                  <button onClick={() => setView('CREATE_RESTAURANT')}>+ Dodaj Restaurację</button>
              </div>
          </div>
      );
  }

  // Domyślnie: LOGIN
  // Uwaga: Musisz zaktualizować swój LoginForm.tsx, żeby przyjmował prop "onRegisterClick"
  return (
    <div style={{textAlign: 'center'}}>
        <LoginForm onSuccess={handleLoginSuccess} />
        <p style={{marginTop: 20}}>
            Nie masz konta? <button onClick={() => setView('REGISTER')} style={{border: 'none', background: 'none', color: 'blue', textDecoration: 'underline', cursor: 'pointer'}}>Zarejestruj się</button>
        </p>
    </div>
  );
}

export default App;