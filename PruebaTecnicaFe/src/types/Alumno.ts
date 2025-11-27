export interface Alumno {
    id: number;
    nombre: string;
    apellido: string;
    email: string;
    fechaNacimiento: string;
}

export type AlumnoCreateDTO = Omit<Alumno, 'id'>;