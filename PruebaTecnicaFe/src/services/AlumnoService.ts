import axios from "axios";
import type { Alumno, AlumnoCreateDTO } from "../types/Alumno";

const API_URL = "https://localhost:7001/api/Alumnos";

export default{
    async getAll(): Promise<Alumno[]> {
        const response =  await axios.get<Alumno[]>(API_URL);
        return response.data;
    },

    async create(alumno: AlumnoCreateDTO): Promise<Alumno> {
        const response = await axios.post<Alumno>(API_URL, alumno);
        return response.data;
    },

    async update(id: number, alumno: AlumnoCreateDTO): Promise<void> {
        await axios.put(`${API_URL}/${id}`, alumno);
    },

    async delete(id: number): Promise<void> {
        await axios.delete(`${API_URL}/${id}`);
    }
}

