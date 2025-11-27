<script setup lang="ts">
import { ref, onMounted } from 'vue';
import type { Alumno, AlumnoCreateDTO } from '@/types/Alumno';
import AlumnoService from '@/services/AlumnoService';
import Swal from 'sweetalert2';

const listaAlumnos = ref<Alumno[]>([]);
const cargando = ref(false);
const editando = ref(false);
const alumnoIdEditando = ref<number | null>(null);

const nuevoAlumno = ref<AlumnoCreateDTO>({
  nombre: '',
  apellido: '',
  email: '',
  fechaNacimiento: ''
});

const formatearFecha = (fecha: string) => {
  return fecha ? new Date(fecha).toLocaleDateString() : '';
};

const cargarDatos = async () => {
  cargando.value = true;
  try {
    listaAlumnos.value = await AlumnoService.getAll();
  } catch (error) {
    console.error(error);
    Swal.fire({
      icon: 'error',
      title: 'Error al cargar datos',
      text: 'No se pudieron cargar los alumnos. Por favor, intenta nuevamente.',
      confirmButtonColor: '#6366f1'
    });
  } finally {
    cargando.value = false;
  }
};

const guardarAlumno = async () => {
  try {
    if (editando.value && alumnoIdEditando.value !== null) {
      await AlumnoService.update(alumnoIdEditando.value, nuevoAlumno.value);
      const index = listaAlumnos.value.findIndex(a => a.id === alumnoIdEditando.value);
      if (index !== -1) {
        listaAlumnos.value[index] = {
          id: alumnoIdEditando.value,
          ...nuevoAlumno.value
        };
      }
      Swal.fire({
        icon: 'success',
        title: '¡Actualizado!',
        text: 'El alumno ha sido actualizado exitosamente.',
        timer: 2000,
        showConfirmButton: false,
        toast: true,
        position: 'top-end'
      });
    } else {
      const creado = await AlumnoService.create(nuevoAlumno.value);
      listaAlumnos.value.push(creado);
      Swal.fire({
        icon: 'success',
        title: '¡Creado!',
        text: 'El alumno ha sido creado exitosamente.',
        timer: 2000,
        showConfirmButton: false,
        toast: true,
        position: 'top-end'
      });
    }
    
    limpiarFormulario();
  } catch (error) {
    console.error(error);
    Swal.fire({
      icon: 'error',
      title: 'Error',
      text: editando.value ? 'No se pudo actualizar el alumno. Por favor, intenta nuevamente.' : 'No se pudo crear el alumno. Por favor, intenta nuevamente.',
      confirmButtonColor: '#6366f1'
    });
  }
};

const editarAlumno = (alumno: Alumno) => {
  editando.value = true;
  alumnoIdEditando.value = alumno.id;
  nuevoAlumno.value = {
    nombre: alumno.nombre,
    apellido: alumno.apellido,
    email: alumno.email,
    fechaNacimiento: alumno.fechaNacimiento
  };
  window.scrollTo({ top: 0, behavior: 'smooth' });
};

const limpiarFormulario = () => {
  nuevoAlumno.value = {
    nombre: '',
    apellido: '',
    email: '',
    fechaNacimiento: ''
  };
  editando.value = false;
  alumnoIdEditando.value = null;
};

const eliminar = async (id: number) => {
  const result = await Swal.fire({
    title: '¿Estás seguro?',
    text: 'Esta acción no se puede deshacer',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#f43f5e',
    cancelButtonColor: '#94a3b8',
    confirmButtonText: 'Sí, eliminar',
    cancelButtonText: 'Cancelar'
  });

  if (!result.isConfirmed) return;

  try {
    await AlumnoService.delete(id);
    listaAlumnos.value = listaAlumnos.value.filter(a => a.id !== id);
    Swal.fire({
      icon: 'success',
      title: '¡Eliminado!',
      text: 'El alumno ha sido eliminado exitosamente.',
      timer: 2000,
      showConfirmButton: false,
      toast: true,
      position: 'top-end'
    });
  } catch (error) {
    console.error(error);
    Swal.fire({
      icon: 'error',
      title: 'Error',
      text: 'No se pudo eliminar el alumno. Por favor, intenta nuevamente.',
      confirmButtonColor: '#6366f1'
    });
  }
};

onMounted(cargarDatos);
</script>

<template>
  <div class="container">
    <div class="header">
      <div class="header-icon">
        <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M22 10v6M2 10l10-5 10 5-10 5z"/>
          <path d="M6 12v5c3 3 9 3 12 0v-5"/>
        </svg>
      </div>
      <h1>Gestión de Alumnos</h1>
      <p class="header-subtitle">Sistema de administración estudiantil</p>
    </div>

    <div class="card card-form">
      <div class="card-header">
        <div class="card-icon">
          <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"/>
            <circle cx="9" cy="7" r="4"/>
            <line x1="19" y1="8" x2="19" y2="14"/>
            <line x1="22" y1="11" x2="16" y2="11"/>
          </svg>
        </div>
        <h2>{{ editando ? 'Editar Alumno' : 'Agregar Nuevo Alumno' }}</h2>
      </div>
      <form @submit.prevent="guardarAlumno" class="form-grid">
        <div class="input-group">
          <label>Nombre</label>
          <input v-model="nuevoAlumno.nombre" placeholder="Ingrese el nombre" required />
        </div>
        <div class="input-group">
          <label>Apellido</label>
          <input v-model="nuevoAlumno.apellido" placeholder="Ingrese el apellido" required />
        </div>
        <div class="input-group">
          <label>Correo Electrónico</label>
          <input v-model="nuevoAlumno.email" type="email" placeholder="correo@ejemplo.com" required />
        </div>
        <div class="input-group">
          <label>Fecha de Nacimiento</label>
          <input v-model="nuevoAlumno.fechaNacimiento" type="date" required />
        </div>
        <button type="submit" class="btn-save">
          <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M19 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11l5 5v11a2 2 0 0 1-2 2z"/>
            <polyline points="17 21 17 13 7 13 7 21"/>
            <polyline points="7 3 7 8 15 8"/>
          </svg>
          {{ editando ? 'Actualizar Alumno' : 'Guardar Alumno' }}
        </button>
        <button v-if="editando" type="button" @click="limpiarFormulario" class="btn-cancel">
          <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <line x1="18" y1="6" x2="6" y2="18"/>
            <line x1="6" y1="6" x2="18" y2="18"/>
          </svg>
          Cancelar
        </button>
      </form>
    </div>

    <div class="card">
      <div class="card-header">
        <div class="card-icon">
          <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/>
            <circle cx="9" cy="7" r="4"/>
            <path d="M23 21v-2a4 4 0 0 0-3-3.87"/>
            <path d="M16 3.13a4 4 0 0 1 0 7.75"/>
          </svg>
        </div>
        <div>
          <h2>Lista de Alumnos</h2>
          <p class="card-description">Total de registros: {{ listaAlumnos.length }}</p>
        </div>
      </div>
      
      <div v-if="cargando" class="loading">
        <div class="spinner"></div>
        <p>Cargando datos...</p>
      </div>
      
      <div v-else-if="listaAlumnos.length === 0" class="empty-state">
        <svg xmlns="http://www.w3.org/2000/svg" width="64" height="64" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
          <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/>
          <circle cx="9" cy="7" r="4"/>
          <path d="M23 21v-2a4 4 0 0 0-3-3.87"/>
          <path d="M16 3.13a4 4 0 0 1 0 7.75"/>
        </svg>
        <h3>No hay alumnos registrados</h3>
        <p>Comienza agregando tu primer alumno usando el formulario superior</p>
      </div>

      <div v-else class="table-container">
        <table>
          <thead>
            <tr>
              <th>
                <div class="th-content">
                  <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
                    <circle cx="12" cy="7" r="4"/>
                  </svg>
                  Nombre Completo
                </div>
              </th>
              <th>
                <div class="th-content">
                  <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"/>
                    <polyline points="22,6 12,13 2,6"/>
                  </svg>
                  Correo Electrónico
                </div>
              </th>
              <th>
                <div class="th-content">
                  <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <rect x="3" y="4" width="18" height="18" rx="2" ry="2"/>
                    <line x1="16" y1="2" x2="16" y2="6"/>
                    <line x1="8" y1="2" x2="8" y2="6"/>
                    <line x1="3" y1="10" x2="21" y2="10"/>
                  </svg>
                  Fecha Nacimiento
                </div>
              </th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="alumno in listaAlumnos" :key="alumno.id">
              <td>
                <div class="user-cell">
                  <div class="avatar">{{ alumno.nombre.charAt(0) }}{{ alumno.apellido.charAt(0) }}</div>
                  <span class="user-name">{{ alumno.nombre }} {{ alumno.apellido }}</span>
                </div>
              </td>
              <td>
                <span class="email-badge">{{ alumno.email }}</span>
              </td>
              <td>
                <span class="date-badge">{{ formatearFecha(alumno.fechaNacimiento) }}</span>
              </td>
              <td>
                <div class="action-buttons">
                  <button @click="editarAlumno(alumno)" class="btn-edit">
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
                      <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
                    </svg>
                    Editar
                  </button>
                  <button @click="eliminar(alumno.id)" class="btn-delete">
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <polyline points="3 6 5 6 21 6"/>
                      <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/>
                      <line x1="10" y1="11" x2="10" y2="17"/>
                      <line x1="14" y1="11" x2="14" y2="17"/>
                    </svg>
                    Eliminar
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<style scoped>
:root {
  --color-bg-primary: #ffffff;
  --color-bg-secondary: #f8fafc;
  --color-bg-tertiary: #f1f5f9;
  --color-bg-quaternary: #e0e7ff;
  --color-text-primary: #0f172a;
  --color-text-secondary: #475569;
  --color-text-muted: #94a3b8;
  --color-border: #e2e8f0;
  --color-border-focus: #cbd5e1;
  --color-primary: #6366f1;
  --color-primary-hover: #4f46e5;
  --color-primary-active: #4338ca;
  --color-primary-light: #eef2ff;
  --color-success: #10b981;
  --color-success-hover: #059669;
  --color-success-active: #047857;
  --color-success-light: #d1fae5;
  --color-danger: #f43f5e;
  --color-danger-hover: #e11d48;
  --color-danger-active: #be123c;
  --color-danger-light: #ffe4e6;
  --color-shadow-sm: 0 1px 2px 0 rgb(0 0 0 / 0.05);
  --color-shadow-md: 0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1);
  --color-shadow-lg: 0 10px 15px -3px rgb(0 0 0 / 0.1), 0 4px 6px -4px rgb(0 0 0 / 0.1);
  --color-shadow-xl: 0 20px 25px -5px rgb(0 0 0 / 0.1), 0 8px 10px -6px rgb(0 0 0 / 0.1);
  --focus-ring: 0 0 0 3px rgb(99 102 241 / 0.1);
}

@media (prefers-color-scheme: dark) {
  :root {
    --color-bg-primary: #0f172a;
    --color-bg-secondary: #020617;
    --color-bg-tertiary: #1e293b;
    --color-bg-quaternary: #312e81;
    --color-text-primary: #f8fafc;
    --color-text-secondary: #cbd5e1;
    --color-text-muted: #64748b;
    --color-border: #1e293b;
    --color-border-focus: #334155;
    --color-primary: #818cf8;
    --color-primary-hover: #6366f1;
    --color-primary-active: #4f46e5;
    --color-primary-light: #1e1b4b;
    --color-success: #34d399;
    --color-success-hover: #10b981;
    --color-success-active: #059669;
    --color-success-light: #064e3b;
    --color-danger: #fb7185;
    --color-danger-hover: #f43f5e;
    --color-danger-active: #e11d48;
    --color-danger-light: #4c0519;
    --color-shadow-sm: 0 1px 2px 0 rgb(0 0 0 / 0.5);
    --color-shadow-md: 0 4px 6px -1px rgb(0 0 0 / 0.5), 0 2px 4px -2px rgb(0 0 0 / 0.5);
    --color-shadow-lg: 0 10px 15px -3px rgb(0 0 0 / 0.5), 0 4px 6px -4px rgb(0 0 0 / 0.5);
    --color-shadow-xl: 0 20px 25px -5px rgb(0 0 0 / 0.5), 0 8px 10px -6px rgb(0 0 0 / 0.5);
    --focus-ring: 0 0 0 3px rgb(129 140 248 / 0.2);
  }
}

.container {
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
  max-width: 1000px;
  margin: 0 auto;
  padding: 2rem 1.5rem 4rem;
  color: var(--color-text-primary);
  min-height: 100vh;
}

.header {
  text-align: center;
  margin: 0 auto 3rem;
  width: 100%;
  max-width: 600px;
}

.header-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 64px;
  height: 64px;
  background: linear-gradient(135deg, var(--color-primary) 0%, var(--color-primary-hover) 100%);
  border-radius: 1rem;
  color: white;
  margin-bottom: 1rem;
  box-shadow: var(--color-shadow-lg);
}

.container h1 {
  font-size: 2.5rem;
  font-weight: 800;
  margin: 0;
  color: var(--color-text-primary);
  letter-spacing: -0.025em;
  background: linear-gradient(135deg, var(--color-primary) 0%, var(--color-primary-hover) 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.header-subtitle {
  color: var(--color-text-secondary);
  font-size: 1rem;
  margin-top: 0.5rem;
}

.card {
  background-color: var(--color-bg-primary);
  border: 1px solid var(--color-border);
  padding: 2rem;
  margin: 0 auto 1.5rem;
  border-radius: 1rem;
  box-shadow: var(--color-shadow-xl);
  transition: all 0.3s ease;
  width: 100%;
}

.card:hover {
  box-shadow: var(--color-shadow-xl), 0 0 0 1px var(--color-border-focus);
}

.card-form {
  background: var(--color-bg-primary);
}

.card-header {
  display: flex;
  align-items: center;
  gap: 0.875rem;
  margin-bottom: 1.5rem;
}

.card-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
  background-color: var(--color-primary-light);
  border-radius: 0.625rem;
  color: var(--color-primary);
  flex-shrink: 0;
}

.card h2 {
  font-size: 1.25rem;
  font-weight: 700;
  margin: 0;
  color: var(--color-text-primary);
}

.card-description {
  color: var(--color-text-muted);
  font-size: 0.875rem;
  margin: 0.25rem 0 0 0;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1.25rem;
}

.input-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.input-group label {
  font-size: 0.875rem;
  font-weight: 600;
  color: var(--color-text-secondary);
  letter-spacing: 0.01em;
}

@media (max-width: 640px) {
  .form-grid {
    grid-template-columns: 1fr;
  }
}

input {
  padding: 0.75rem 1rem;
  border: 1.5px solid var(--color-border);
  border-radius: 0.625rem;
  font-size: 0.9375rem;
  color: var(--color-text-primary);
  background-color: var(--color-bg-primary);
  transition: all 0.2s ease;
  outline: none;
}

input::placeholder {
  color: var(--color-text-muted);
}

input:hover {
  border-color: var(--color-border-focus);
}

input:focus {
  border-color: var(--color-primary);
  box-shadow: var(--focus-ring);
  background-color: var(--color-bg-primary);
}

button {
  cursor: pointer;
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 0.625rem;
  color: white;
  font-weight: 600;
  font-size: 0.9375rem;
  transition: all 0.2s ease;
  outline: none;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}

button:active {
  transform: translateY(1px);
}

button:focus-visible {
  box-shadow: var(--focus-ring);
}

.btn-save {
  background: linear-gradient(135deg, var(--color-success) 0%, var(--color-success-hover) 100%);
  grid-column: span 2;
  padding: 0.875rem 1.5rem;
  font-size: 1rem;
  box-shadow: var(--color-shadow-md);
}

.btn-save:hover {
  background: linear-gradient(135deg, var(--color-success-hover) 0%, var(--color-success-active) 100%);
  box-shadow: var(--color-shadow-lg);
  transform: translateY(-2px);
}

.btn-save:active {
  transform: translateY(0);
}

@media (max-width: 640px) {
  .btn-save {
    grid-column: span 1;
  }
}

.btn-cancel {
  background: linear-gradient(135deg, var(--color-text-muted) 0%, var(--color-text-secondary) 100%);
  grid-column: span 2;
  padding: 0.875rem 1.5rem;
  font-size: 1rem;
  box-shadow: var(--color-shadow-md);
}

.btn-cancel:hover {
  background: linear-gradient(135deg, var(--color-text-secondary) 0%, var(--color-text-primary) 100%);
  box-shadow: var(--color-shadow-lg);
  transform: translateY(-2px);
}

@media (max-width: 640px) {
  .btn-cancel {
    grid-column: span 1;
  }
}

.btn-edit {
  background: linear-gradient(135deg, var(--color-primary) 0%, var(--color-primary-hover) 100%);
  padding: 0.5rem 1rem;
  font-size: 0.8125rem;
}

.btn-edit:hover {
  background: linear-gradient(135deg, var(--color-primary-hover) 0%, var(--color-primary-active) 100%);
  box-shadow: var(--color-shadow-md);
  transform: translateY(-1px);
}

.btn-edit:active {
  transform: translateY(0);
}

.action-buttons {
  display: flex;
  gap: 0.5rem;
  flex-wrap: wrap;
}

.btn-delete {
  background: linear-gradient(135deg, var(--color-danger) 0%, var(--color-danger-hover) 100%);
  padding: 0.5rem 1rem;
  font-size: 0.8125rem;
}

.btn-delete:hover {
  background: linear-gradient(135deg, var(--color-danger-hover) 0%, var(--color-danger-active) 100%);
  box-shadow: var(--color-shadow-md);
  transform: translateY(-1px);
}

.btn-delete:active {
  transform: translateY(0);
}

.loading {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 3rem 1rem;
  color: var(--color-text-secondary);
  gap: 1rem;
}

.spinner {
  width: 40px;
  height: 40px;
  border: 3px solid var(--color-border);
  border-top-color: var(--color-primary);
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 4rem 2rem;
  text-align: center;
  color: var(--color-text-muted);
}

.empty-state svg {
  margin-bottom: 1.5rem;
  opacity: 0.5;
}

.empty-state h3 {
  font-size: 1.125rem;
  font-weight: 600;
  color: var(--color-text-secondary);
  margin: 0 0 0.5rem 0;
}

.empty-state p {
  font-size: 0.875rem;
  margin: 0;
  max-width: 400px;
}

.table-container {
  overflow-x: auto;
  margin-top: 1rem;
  border-radius: 0.75rem;
  border: 1px solid var(--color-border);
}

table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.875rem;
}

thead {
  background: linear-gradient(135deg, var(--color-bg-tertiary) 0%, var(--color-bg-secondary) 100%);
}

thead tr th {
  text-transform: uppercase;
  font-size: 0.6875rem;
  font-weight: 700;
  letter-spacing: 0.075em;
  color: var(--color-text-secondary);
}

.th-content {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

th,
td {
  text-align: left;
  padding: 1rem 1.25rem;
  border-bottom: 1px solid var(--color-border);
}

tbody tr {
  transition: all 0.15s ease;
}

tbody tr:hover {
  background-color: var(--color-bg-tertiary);
  transform: scale(1.005);
}

tbody tr:last-child td {
  border-bottom: none;
}

td {
  color: var(--color-text-primary);
}

.user-cell {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.avatar {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: linear-gradient(135deg, var(--color-primary) 0%, var(--color-primary-hover) 100%);
  color: white;
  font-weight: 700;
  font-size: 0.75rem;
  flex-shrink: 0;
}

.user-name {
  font-weight: 600;
  color: var(--color-text-primary);
}

.email-badge {
  display: inline-flex;
  padding: 0.375rem 0.75rem;
  background-color: var(--color-bg-tertiary);
  border-radius: 0.375rem;
  font-size: 0.8125rem;
  color: var(--color-text-secondary);
  border: 1px solid var(--color-border);
}

.date-badge {
  display: inline-flex;
  padding: 0.375rem 0.75rem;
  background-color: var(--color-primary-light);
  border-radius: 0.375rem;
  font-size: 0.8125rem;
  color: var(--color-primary);
  font-weight: 600;
  border: 1px solid var(--color-primary);
  border-opacity: 0.2;
}

@media (max-width: 768px) {
  .container {
    padding: 1.5rem 1rem 3rem;
  }

  .header-icon {
    width: 56px;
    height: 56px;
  }

  .container h1 {
    font-size: 2rem;
  }

  .card {
    padding: 1.5rem;
  }

  .table-container {
    font-size: 0.8125rem;
  }

  th,
  td {
    padding: 0.75rem;
  }
}
</style>