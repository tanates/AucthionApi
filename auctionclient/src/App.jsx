import { useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import AuctionPage from '../Main/Page/AllAuctinonePage';

function App() {
    const [count, setCount] = useState(0)

    return (
        <Router>
            <Routes>
                <Route path="/main" element={<AuctionPage />} />
                {/* Вы можете добавить другие маршруты здесь */}
            </Routes>
        </Router>
    );
}

export default App
