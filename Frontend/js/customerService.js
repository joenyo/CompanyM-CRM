// Service for handling API calls related to customers
const customerService = {
    apiUrl: 'http://localhost:5163/api/Customers',
    
    // Get all customers
    getAllCustomers: async function() {
        try {
            const response = await fetch(this.apiUrl);
            if (!response.ok) {
                throw new Error('Failed to fetch customers');
            }
            return await response.json();
        } catch (error) {
            console.error('Error getting customers:', error);
            throw error;
        }
    },
    
    // Get customer by ID
    getCustomerById: async function(id) {
        try {
            const response = await fetch(`${this.apiUrl}/${id}`);
            if (!response.ok) {
                throw new Error('Failed to fetch customer');
            }
            return await response.json();
        } catch (error) {
            console.error(`Error getting customer ${id}:`, error);
            throw error;
        }
    },
    
    // Create a new customer
    createCustomer: async function(customerData) {
        try {
            const response = await fetch(this.apiUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(customerData)
            });
            
            if (!response.ok) {
                throw new Error('Failed to create customer');
            }
            
            return await response.json();
        } catch (error) {
            console.error('Error creating customer:', error);
            throw error;
        }
    },
    
    // Search customers by name
    searchCustomers: async function(name) {
        try {
            const response = await fetch(`${this.apiUrl}/Search/${encodeURIComponent(name)}`);
            if (!response.ok) {
                throw new Error('Failed to search customers');
            }
            return await response.json();
        } catch (error) {
            console.error('Error searching customers:', error);
            throw error;
        }
    }
};

// Service for handling API calls related to channel types
const channelService = {
    apiUrl: 'http://localhost:5163/api/ChannelTypes',
    
    // Get all channel types
    getAllChannels: async function() {
        try {
            const response = await fetch(this.apiUrl);
            if (!response.ok) {
                throw new Error('Failed to fetch channel types');
            }
            return await response.json();
        } catch (error) {
            console.error('Error getting channel types:', error);
            throw error;
        }
    }
};