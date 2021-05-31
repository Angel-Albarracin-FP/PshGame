import React, { useEffect, useState } from 'react';
import { service } from "./_services/service";
import ListStats from './ListStats';

function App() {
  const [topAllTime, setTopAllTime] = useState([]);
  const [topLastTime, setTopLastTime] = useState([]);

  useEffect(() => {
    loadStats();
    const interval = setInterval(loadStats, 5000);
    return () => clearInterval(interval);
  }, [])

  const loadStats = () => {
    service.topAllTime().then(setTopAllTime);
    service.topLastTime().then(setTopLastTime);
  }

  const downloadCSV = (csv, filename) => {
    var csvFile;
    var downloadLink;
    csvFile = new Blob([csv], { type: "text/csv" });
    downloadLink = document.createElement("a");
    downloadLink.download = filename;
    downloadLink.href = window.URL.createObjectURL(csvFile);
    downloadLink.style.display = "none";
    document.body.appendChild(downloadLink);
    downloadLink.click();
  }

  const exportData = (data, filename) => {
    let csv = [];

    let row = ["Nick", "Puntaje", "Fecha"];
    csv.push(row.join(","));

    data.forEach(stat => {
      row = [stat.user.userName, stat.score, stat.created];
      csv.push(row.join(","));
    });

    // Download CSV file
    downloadCSV(csv.join("\n"), filename + ".csv");
  }

  return (
    <div className='container-fluid'>
      <h2>Top 10 All Time</h2>
      <button className="btn btn-primary" onClick={() => exportData(topAllTime, "topAllTime")}>Exportar</button>
      <ListStats data={topAllTime} />
      <h2 className="mt-5">Top 10 Last Time</h2>
      <button className="btn btn-primary" onClick={() => exportData(topLastTime, "topLastTime")}>Exportar</button>
      <ListStats data={topLastTime} />
    </div>
  );
}

export default App;
