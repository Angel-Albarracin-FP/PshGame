import React from 'react'

export default function ListStats({ data }) {
  return (
    <table className="table">
      <thead>
        <tr>
          <th>Avatar</th>
          <th>Nick</th>
          <th>Puntaje</th>
          <th>Fecha</th>
        </tr>
      </thead>
      <tbody>
        {data.map((stat) => (
          <tr>
            <td><img src={stat.user.picture} alt="Avatar" /></td>
            <td>{stat.user.userName}</td>
            <td>{stat.score}</td>
            <td>{stat.created}</td>
          </tr>
        ))}
      </tbody>
    </table>
  )
}
