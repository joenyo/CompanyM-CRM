// Service for handling API calls related to cases
const caseService = {
    apiUrl: 'http://localhost:5163/api/Cases',
    
    // Get all cases
    getAllCases: async function() {
        try {
            const response = await fetch(this.apiUrl);
            if (!response.ok) {
                throw new Error('Failed to fetch cases');
            }
            return await response.json();
        } catch (error) {
            console.error('Error getting cases:', error);
            throw error;
        }
    },
    
    // Search cases by customer name and/or channel
    searchCases: async function(customerName, channelId) {
        try {
            let url = `${this.apiUrl}/Search?`;
            
            if (customerName) {
                url += `customerName=${encodeURIComponent(customerName)}&`;
            }
            
            if (channelId) {
                url += `channelId=${channelId}`;
            }
            
            const response = await fetch(url);
            if (!response.ok) {
                throw new Error('Failed to search cases');
            }
            return await response.json();
        } catch (error) {
            console.error('Error searching cases:', error);
            throw error;
        }
    },
    
    // Get case by ID
    getCaseById: async function(id) {
        try {
            const response = await fetch(`${this.apiUrl}/${id}`);
            if (!response.ok) {
                throw new Error('Failed to fetch case');
            }
            return await response.json();
        } catch (error) {
            console.error(`Error getting case ${id}:`, error);
            throw error;
        }
    },
    
    // Create a new case
    createCase: async function(caseData) {
        try {
            const response = await fetch(this.apiUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(caseData)
            });
            
            if (!response.ok) {
                throw new Error('Failed to create case');
            }
            
            return await response.json();
        } catch (error) {
            console.error('Error creating case:', error);
            throw error;
        }
    },
    
    // Update an existing case
    updateCase: async function(id, caseData) {
        try {
            const response = await fetch(`${this.apiUrl}/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(caseData)
            });
            
            if (!response.ok) {
                throw new Error('Failed to update case');
            }
            
            return true;
        } catch (error) {
            console.error(`Error updating case ${id}:`, error);
            throw error;
        }
    },
    
    // Delete a case
    deleteCase: async function(id) {
        try {
            const response = await fetch(`${this.apiUrl}/${id}`, {
                method: 'DELETE'
            });
            
            if (!response.ok) {
                throw new Error('Failed to delete case');
            }
            
            return true;
        } catch (error) {
            console.error(`Error deleting case ${id}:`, error);
            throw error;
        }
    }
};