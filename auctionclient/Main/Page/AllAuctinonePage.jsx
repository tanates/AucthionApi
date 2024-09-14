import React, { useState, useEffect } from 'react';
import axios from 'axios';
import AuctionItem from "../Type/AuctionType.jsx";
import ApiClient from "../Api/ApiClass.jsx";

const AuctionPage = () => {
    const [items, setItems] = useState([]);
    const apiClient = new ApiClient();
    // Функция для получения данных из API
    const fetchItems = async () => {
        try {
            const response = await apiClient.postRequest('/api/auction/getItems', 'AuctionService',null);
            const fetchedItems = response.Data.map(item => ({
                id: item.id,
                name: item.name,
                price: item.price
            }));
            setItems(fetchedItems);
        } catch (error) {
            console.error('Error fetching auction items:', error);
        }
    };

    // Запрашиваем данные при монтировании компонента
    useEffect(() => {
        fetchItems();
    }, []);

    // Обработчик покупки
    const handleBuyClick = (id) => {
        alert(`Buying item with id: ${id}`);
        // Здесь можно добавить логику для обработки покупки
    };

    return (

        <div className="flex-wrap">
            <h1>Auction Items</h1>
            <div >
                <button type="button" className="btn btn-secondary">Creat Auctione</button>
            </div>

            <div className="flex-wrap">
                {items?.length > 0 ? (
                    items.map((item) => (
                        <div key={item.id} style={{ border: '1px solid #ccc', margin: '10px', padding: '10px', width: '300px' }}>
                            <h2>{item.name}</h2>
                            <p>Start Price: ${item.startPrice}</p>
                            <p>End Price: ${item.endPrice}</p>
                            <p>Start Time: {new Date(item.startTime).toLocaleString()}</p>
                            <p>End Time: {new Date(item.endTime).toLocaleString()}</p>
                            <p>Description: {item.description}</p>
                            <button onClick={() => handleBuyClick(item.id)}>Buy Now</button>
                        </div>
                    ))
                ) : (
                    <p>List is empty</p>
                )
                }
            </div>
        </div>
    );
};


export default AuctionPage;