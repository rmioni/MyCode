import { useEffect, useState } from 'react';
import './Weather.css';

// 1. Define the shape of a single Forecast object
interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

function Weather() {
    // 2. Type the state as an array of Forecasts or undefined
    // Means Forecast[] forecasts = undefined;
    // setForecasts is the only way to set it
    const [forecasts, setForecasts] = useState<Forecast[] | undefined>();

    useEffect(() => // Run this function every time any value in the dependency list changes
        { populateWeatherData(); },
        [] // a dependency list - func will run if any of the variables in it change. empty [] means run exactly once (on page refresh)
    );

    // TypeScript now knows that if forecasts is not undefined, it's an array we can map
    const contents = forecasts === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started.</em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map(forecast =>
                    <tr key={forecast.date}>
                        <td>{forecast.date}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tableLabel" style={{ color: 'purple' }}>Weather forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );

    // 3. The async function now expects the JSON to match our Forecast interface
    async function populateWeatherData() {
        // NOTE: Ensure this port matches your actual backend port!
        const response = await fetch('https://localhost:7132/weatherforecast');

        if (response.ok) {
            const forecasts: Forecast[] = await response.json();
            setForecasts(forecasts);
        }
    }
}

export default Weather;