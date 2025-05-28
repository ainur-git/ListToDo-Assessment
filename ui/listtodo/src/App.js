import React, { useState, useEffect } from 'react';
import axios from 'axios';
import styles from './App.css';

const API_URL = 'http://localhost:5189/api/todolist';

const App = () => 
{
  const [lists, setLists] = useState([]);

  const fetchToDoLists = async () => 
  {
    try 
    {
      const res = await axios.get(API_URL);
      setLists(res.data);
    } 
    catch (err) 
    {
      console.error('Error fetching to-do lists:', err);
    }
  };

  return (
  <div className={styles.container}>
    <h1 className={styles.header}>All To-Do Lists</h1>

    <button onClick={fetchToDoLists} className={styles.button}>
      Load Lists
    </button>

    {lists.map(list => (
      <div key={list.listToDoId} className={styles.listCard}>
        <h2 className={styles.listTitle}>{list.title}</h2>
        <p>{list.description}</p>
        <ul className={styles.listItems}>
          {list.items.map(item => (
            <li key={item.itemToDoId}>
              <strong>{item.title}</strong>: {item.description} (Due: {item.dueDate?.split('T')[0]}) - 
              <span className={item.isCompleted ? styles.completed : styles.pending}>
                {item.isCompleted ? ' Completed' : ' Pending'}
              </span>
            </li>
          ))}
        </ul>
      </div>
    ))}
  </div>);
};


export default App;


