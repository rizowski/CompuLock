class AccountProcess < ActiveRecord::Base
    attr_accessible :account_id, :name

	  validates :account_id, presence: true
    validates :name, presence: true, uniqueness: {scope: :account_id}
    
    belongs_to :account
    def as_json options={}
    {
      id: id,
      name: name,
      
      created_at: created_at,
      update_at: updated_at

    }
  end
end
