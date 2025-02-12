//Get date with JS
const currentDate = new Date();
const options = { weekday: 'long' };
const dayOfWeek = currentDate.toLocaleDateString('en-US', options);

const optionsDate = { month: 'short', day: 'numeric' };
const formattedDate = currentDate.toLocaleDateString('en-US', optionsDate);

const hours = currentDate.getHours();
const minutes = currentDate.getMinutes().toString().padStart(2, '0');
const seconds = currentDate.getSeconds().toString().padStart(2, '0');
const period = hours >= 12 ? 'PM' : 'AM'; // Determine AM or PM
const formattedTime = `${(hours % 12 || 12)}:${minutes}:${seconds} ${period}`;

const year = currentDate.getFullYear();

const finalFormattedDate = `${dayOfWeek}, ${formattedDate}, ${formattedTime}, ${year}`;

// Set current-time to date.
document.getElementById('current-time').textContent = finalFormattedDate;