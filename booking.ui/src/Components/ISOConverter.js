const FormatToReadableDate = (isoString) => {
    if (!isoString) return '';
    
    const date = new Date(isoString);
    
    return date.toLocaleString('ru-RU', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
};

export default FormatToReadableDate