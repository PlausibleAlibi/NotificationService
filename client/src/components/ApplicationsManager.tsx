import { useState, useEffect } from 'react';
import {
  Box,
  Button,
  Card,
  CardContent,
  CardActions,
  Typography,
  TextField,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Grid,
  Chip,
  IconButton,
} from '@mui/material';
import { Add as AddIcon, Delete as DeleteIcon } from '@mui/icons-material';
import { applicationsApi } from '../services/api';
import type { Application, CreateApplicationDto } from '../types';

interface ApplicationsManagerProps {
  tenantId: number;
}

export default function ApplicationsManager({ tenantId }: ApplicationsManagerProps) {
  const [applications, setApplications] = useState<Application[]>([]);
  const [showDialog, setShowDialog] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  const [formData, setFormData] = useState<CreateApplicationDto>({
    tenantId,
    code: '',
    name: '',
    description: '',
  });

  useEffect(() => {
    loadApplications();
  }, [tenantId]);

  const loadApplications = async () => {
    setLoading(true);
    try {
      const data = await applicationsApi.getByTenant(tenantId);
      setApplications(data);
      setError('');
    } catch {
      setError('Failed to load applications');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async () => {
    setLoading(true);
    setError('');

    try {
      await applicationsApi.create(formData);
      setShowDialog(false);
      setFormData({
        tenantId,
        code: '',
        name: '',
        description: '',
      });
      await loadApplications();
    } catch {
      setError('Failed to create application');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Are you sure you want to delete this application?')) return;

    try {
      await applicationsApi.delete(id);
      await loadApplications();
    } catch {
      setError('Failed to delete application');
    }
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h5" component="h2">
          Applications
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => setShowDialog(true)}
        >
          Add Application
        </Button>
      </Box>

      {error && (
        <Typography color="error" sx={{ mb: 2 }}>
          {error}
        </Typography>
      )}

      <Grid container spacing={2}>
        {applications.map((app) => (
          <Grid item xs={12} md={6} lg={4} key={app.id}>
            <Card>
              <CardContent>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'start' }}>
                  <Typography variant="h6" component="div">
                    {app.name}
                  </Typography>
                  <Chip
                    label={app.isActive ? 'Active' : 'Inactive'}
                    color={app.isActive ? 'success' : 'default'}
                    size="small"
                  />
                </Box>
                <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                  Code: {app.code}
                </Typography>
                <Typography variant="body2" sx={{ mt: 1 }}>
                  {app.description}
                </Typography>
              </CardContent>
              <CardActions>
                <IconButton size="small" color="error" onClick={() => handleDelete(app.id)}>
                  <DeleteIcon />
                </IconButton>
              </CardActions>
            </Card>
          </Grid>
        ))}
      </Grid>

      <Dialog open={showDialog} onClose={() => setShowDialog(false)} maxWidth="sm" fullWidth>
        <DialogTitle>Create Application</DialogTitle>
        <DialogContent>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 2 }}>
            <TextField
              label="Code"
              value={formData.code}
              onChange={(e) => setFormData({ ...formData, code: e.target.value })}
              required
              fullWidth
            />
            <TextField
              label="Name"
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              required
              fullWidth
            />
            <TextField
              label="Description"
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              multiline
              rows={3}
              fullWidth
            />
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setShowDialog(false)}>Cancel</Button>
          <Button onClick={handleSubmit} variant="contained" disabled={loading}>
            Create
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}
